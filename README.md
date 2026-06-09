# GenericWorkflowAPI

GenericWorkflowAPI is a .NET 10 solution for defining and running configurable state-machine workflows through a versioned REST API. The application lets clients maintain workflow metadata (types, workflows, states, transitions, input-code definitions), execute workflow instances, and inspect runtime instance data and history. The solution also includes an IdentityServer4 host for authentication flows, an Entity Framework Core persistence layer, unit tests, and mapping/benchmark projects.

## Solution layout

| Project | Purpose |
| --- | --- |
| `GenericWorkflowAPI` | Main ASP.NET Core Web API. It exposes the workflow controllers, configures API versioning, Swagger/OpenAPI, OData query support, JWT bearer authentication, antiforgery protection, Serilog logging, MediatR handlers, AutoMapper, repositories, and workflow execution services. |
| `GenericWorkflowAPI.Domain` | Domain entities, DTOs, request/response contracts, interfaces, identity types, constants, and helper extensions used by the API and persistence layers. |
| `GenericWorkflowAPI.Database` | Entity Framework Core database contexts, migrations, and entity-to-DTO AutoMapper profiles for SQL Server and SQLite-backed persistence. |
| `GenericWorkflowAPI.Core` | Shared infrastructure such as attributes, filters, logging helpers, service interfaces, AutoMapper mapping discovery, host-building helpers, and MVC/OData/OpenAPI filters. |
| `GenericWorkflowAPI.IdentityServer4` | IdentityServer4 MVC host for login/logout, consent, device authorization, external authentication, persisted grants, diagnostics, and OAuth/OpenID Connect configuration. |
| `GenericWorkflowAPI.UnitTesting` | MSTest-based tests for controllers, generic command handlers, repositories, and service registration pairs. |
| `GenericWorkflowAPI.Benchmark` | BenchmarkDotNet project for measuring mapping performance. |

## Architecture at a glance

The main API uses thin controllers backed by generic request handlers. Controllers translate HTTP calls into MediatR requests, handlers perform generic CRUD/list/query operations, repositories persist entities with EF Core, and AutoMapper converts between entity and DTO models. Specialized workflow execution is handled by `WorkflowController.ExecuteWorkflow`, `ExecuteWorkflowCommandHandler`, and `WorkflowService`.

Important runtime building blocks:

- **Versioned REST routes**: Controllers inherit the route template `api/v{version:ApiVersion}/[controller]s`, so `WorkflowController` becomes `/api/v1/Workflows`, `WorkflowStateController` becomes `/api/v1/WorkflowStates`, and so on.
- **JWT authentication**: The API can validate bearer tokens issued by the configured IdentityServer authority when `Authentication:UseAuthentication` is `true`.
- **OData query endpoints**: Each exposed resource can provide a `GET Queryable` endpoint with OData query options over DTOs.
- **Generic data access**: CRUD controllers reuse generic create, update, delete, get, list, and query handlers instead of duplicating persistence logic per entity.
- **Problem Details and logging**: Exceptions are logged with Serilog and returned through Problem Details middleware.
- **Database support**: Configuration chooses SQL Server or SQLite, and EF Core contexts contain workflow and ASP.NET Identity tables.

## Workflow domain model

The workflow model is designed around codes rather than database IDs at the API boundary:

- **Workflow types** categorize workflows.
- **Workflows** belong to a workflow type and are the top-level process definition.
- **Workflow states** belong to a workflow and mark possible positions in the process. One state is expected to be the first state for new instances.
- **Workflow transitions** connect a current state to a next state and require a role code before a user can execute that transition.
- **Workflow input code types** define named input values that may be supplied when starting or advancing a workflow.
- **Workflow state input code types** associate required input-code definitions with a particular state.
- **Workflow instances** track one running instance of a workflow and its current state.
- **Workflow instance input codes** store the latest value for each input code on an instance.
- **Workflow instance histories** record state movement for an instance.
- **Workflow instance history input codes** record the input values captured for a historical execution step.

## Main API controllers and features

All main API controllers are API version `1.0`. They inherit `[Authorize]`, Serilog logging, and the base route template `api/v{version:ApiVersion}/[controller]s` from the generic base controller. Most write endpoints use `[ValidateAntiForgeryToken]`.

### Generic controller behavior

| Base controller | Used for | Exposed behavior |
| --- | --- | --- |
| `GenericOnlyGetAllController<TEntity,TDto>` | Read-only collection resources | `GET /api/v1/{Resources}` for a list and `GET /api/v1/{Resources}/Queryable` for OData-enabled querying. |
| `GenericGetController<TEntity,TDto>` | Read-only resources addressable by code | Adds `GET /api/v1/{Resources}/{code}`. |
| `GenericCRUDController<TEntity,TDto>` | Mutable resources addressable by code | Adds single and bulk create/update/delete helpers: `POST`, `PUT`, and `DELETE` endpoints for one DTO/code or collections. |

### Workflow definition endpoints

These controllers manage the configuration required to define reusable workflows. Each supports:

- `GET /api/v1/{Resources}/{code}` - get one item by code.
- `GET /api/v1/{Resources}` - get all items.
- `GET /api/v1/{Resources}/Queryable` - query items through OData.
- `POST /api/v1/{Resources}/{code}` - create one item from a DTO body.
- `POST /api/v1/{Resources}` - create a collection of DTOs.
- `PUT /api/v1/{Resources}/{code}` - update one item from a DTO body.
- `PUT /api/v1/{Resources}` - update a collection of DTOs.
- `DELETE /api/v1/{Resources}/{code}` - delete one item by code.
- `DELETE /api/v1/{Resources}` - delete a collection of codes.

| Controller | Route base | Resource managed | Notes |
| --- | --- | --- | --- |
| `WorkflowTypeController` | `/api/v1/WorkflowTypes` | `WorkflowTypeDto` | Workflow categories with code, name, and description. |
| `WorkflowController` | `/api/v1/Workflows` | `WorkflowDto` | Top-level workflow definitions. Also exposes workflow execution. |
| `WorkflowInputCodeTypeController` | `/api/v1/WorkflowInputCodeTypes` | `WorkflowInputCodeTypeDto` | Defines named input values available to a workflow. |
| `WorkflowStateController` | `/api/v1/WorkflowStates` | `WorkflowStateDto` | Defines states in a workflow, including the first-state flag. |
| `WorkflowStateInputCodeTypeController` | `/api/v1/WorkflowStateInputCodeTypes` | `WorkflowStateInputCodeTypeDto` | Associates required input-code types with workflow states. |
| `WorkflowTransitionController` | `/api/v1/WorkflowTransitions` | `WorkflowTransitionDto` | Defines state-to-state transitions and the role required to execute each transition. |

### Workflow runtime endpoints

Runtime controllers expose workflow instance state and audit data. They intentionally do not offer public CRUD operations except through the workflow execution command.

| Controller | Route base | Supported endpoints | Purpose |
| --- | --- | --- | --- |
| `WorkflowInstanceController` | `/api/v1/WorkflowInstances` | `GET {code}`, `GET`, `GET Queryable` | Reads running workflow instances and their current state. |
| `WorkflowInstanceInputCodeController` | `/api/v1/WorkflowInstanceInputCodes` | `GET`, `GET Queryable` | Reads the current input-code values stored on workflow instances. |
| `WorkflowInstanceHistoryController` | `/api/v1/WorkflowInstanceHistorys` | `GET`, `GET Queryable` | Reads historical state changes for workflow instances. The pluralized route follows the controller route template and therefore uses `Historys`. |
| `WorkflowInstanceHistoryInputCodeController` | `/api/v1/WorkflowInstanceHistoryInputCodes` | `GET`, `GET Queryable` | Reads input-code values captured for historical workflow execution steps. |

### Workflow execution endpoint

`WorkflowController` adds a specialized command endpoint:

```http
POST /api/v1/Workflows/ExecuteWorkflow
Content-Type: application/json

{
  "workflowCode": "example-workflow",
  "workflowInstanceCode": "example-instance-001",
  "workflowInputCodeTypeXvalue": {
    "input-code-a": "value-a",
    "input-code-b": "value-b"
  }
}
```

Execution behavior:

1. The request is mapped from `ExecuteWorkflowRequestDto` into an `ExecuteWorkflowRequest` and enriched with the authenticated user.
2. If the supplied `workflowInstanceCode` does not exist, the service creates a new workflow instance at the workflow's first state after validating the provided input-code values for that state.
3. If the instance already exists, the service finds executable transitions from the current state, validates the request input codes against transition/next-state requirements, verifies the current user has the transition's required role, records history, updates the instance's current state, and stores current and historical input-code values.
4. The endpoint returns `201 Created` when execution completes successfully.

## IdentityServer4 application

The `GenericWorkflowAPI.IdentityServer4` project is an MVC IdentityServer host backed by the same Identity user and role model. Its controllers provide:

| Controller | Feature |
| --- | --- |
| `AccountController` | Local login, logout, and access-denied screens. |
| `ConsentController` | User consent workflow for authorization requests. |
| `DeviceController` | Device-flow user-code capture and callback handling. |
| `ExternalController` | External provider challenge and callback flow. |
| `GrantsController` | Viewing and revoking persisted grants. |
| `DiagnosticsController` | Authenticated diagnostics and token inspection helpers. |
| `HomeController` | Home page and IdentityServer error display. |

The IdentityServer configuration includes OpenID Connect identity resources for `openid` and `profile`, an API scope/audience named `GenericWorkflowAPI`, and an in-memory machine-to-machine client using the client credentials grant.

## API support features

- **Swagger/OpenAPI**: In development, Swagger UI is available from the API host and documents the `v1.0` API with bearer-token security metadata.
- **API versioning**: URL-segment versioning is configured with a default API version of `1.0`.
- **OData**: DTOs are registered in an EDM model and queryable endpoints support OData options such as filtering, ordering, paging, selecting, and expanding where enabled on DTO attributes.
- **Antiforgery**: Write actions use anti-forgery validation. In development, middleware emits an `XSRF-TOKEN` cookie and adds a request verification header to make Swagger testing easier.
- **CORS**: CORS is configured through shared service extensions.
- **Logging**: Serilog is configured from application settings and writes to the console plus SQLite or SQL Server sinks depending on configuration.
- **Problem details**: API exceptions are returned with Problem Details responses, including exception details in Development or Staging.

## Configuration and persistence

The API reads configuration from `appsettings.json` plus environment-specific settings. Important settings include:

- `Authentication:UseAuthentication` - enables JWT bearer authentication when set to `true`.
- `Authentication:Issuer`, `Authentication:Audience`, and `Authentication:BaseAddress` - configure IdentityServer token validation.
- `ConnectionStrings:DefaultConnection` - database connection string.
- `SqlType` - selects SQL Server or SQLite-specific registrations.
- `Serilog` - controls database logging sinks and minimum levels.

EF Core contexts expose DbSets for all workflow resources plus ASP.NET Identity tables. Migrations are included for SQL Server and SQLite.

## Running locally

Prerequisites:

- .NET 10 SDK/runtime compatible with the solution target framework.
- SQL Server or SQLite, depending on `SqlType` and connection-string settings.

Common commands:

```bash
# Restore and build the solution
cd src
dotnet restore GenericWorkflowAPI.sln
dotnet build GenericWorkflowAPI.sln

# Run the API host
cd GenericWorkflowAPI
dotnet run

# Run the IdentityServer host in another terminal if JWT authentication is enabled
cd ../GenericWorkflowAPI.IdentityServer4
dotnet run

# Run unit tests
cd ..
dotnet test GenericWorkflowAPI.sln
```

When running with the default authentication settings, start IdentityServer at the configured `Authentication:BaseAddress` before calling protected API endpoints.

## Typical setup flow

A typical workflow definition and execution sequence is:

1. Create a workflow type.
2. Create a workflow using that workflow type code.
3. Create workflow input-code types for the values callers must supply.
4. Create states for the workflow and mark one as `IsFirstState = true`.
5. Create state/input-code associations to define required values for starting and advancing the workflow.
6. Create transitions from state to state with role codes that authorize each movement.
7. Call `POST /api/v1/Workflows/ExecuteWorkflow` with a workflow code, instance code, and input values to create a new instance or advance an existing one.
8. Use the workflow instance and history read endpoints to inspect current state and audit trail.
