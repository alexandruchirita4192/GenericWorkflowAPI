using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GenericWorkflowAPI.CommandHandlers;
using GenericWorkflowAPI.Controllers.v1;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.Helpers
{
    /// <summary>
    /// Gets MediatR handlers based on controllers with their generic arguments
    /// </summary>
    public static class MediatorHelper
    {
        public static List<InterfaceImplementationMapper> GetMappings(ILogger logger)
        {
            var mappings = new List<InterfaceImplementationMapper>();

            var types = GetControllerTypes();
            if (types == null || types.Count == 0)
                return mappings;

            foreach (var type in types)
            {
                if (type.BaseType?.GetGenericArguments()?.Length != 2)
                    continue;

                var entityType = type.BaseType.GetGenericArguments()[0];
                var dtoType = type.BaseType.GetGenericArguments()[1];

                // Example: WorkflowController : GenericCRUDController<Workflow, WorkflowDto>
                var genericCrudControllerType = typeof(GenericCRUDController<,>).MakeGenericType(entityType, dtoType);
                if (genericCrudControllerType.IsAssignableFrom(type))
                {
                    // Example handling input GenericApiResponse<string>:
                    var genericApiResponseHandlerStringMapper = new InterfaceImplementationMapper(
                            typeof(IRequestHandler<GenericApiResponse<string>, ActionResult>),
                            typeof(GenericApiResponseHandler<string>)
                        );
                    if (!mappings.Contains(genericApiResponseHandlerStringMapper))
                        mappings.Add(genericApiResponseHandlerStringMapper);

                    // Example handling input GenericCreateListRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericCreateListRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericCreateListCommandHandler<Workflow, WorkflowDto>));

                    // GenericCreateListRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericCreateListRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(GenericApiResponse<string>),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericCreateListCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);

                    // Example handling input GenericCreateRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericCreateRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericCreateCommandHandler<Workflow, WorkflowDto>));

                    // GenericCreateRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericCreateRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(GenericApiResponse<string>),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericCreateCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);

                    // Example handling input GenericDeleteListRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericDeleteListRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericDeleteListCommandHandler<Workflow, WorkflowDto>));

                    // GenericDeleteListRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericDeleteListRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(GenericApiResponse<string>),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericDeleteListCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);

                    // Example handling input GenericDeleteRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericDeleteRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericDeleteCommandHandler<Workflow, WorkflowDto>));

                    // GenericDeleteRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericDeleteRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(GenericApiResponse<string>),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericDeleteCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);

                    // Example handling input GenericUpdateListRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericUpdateListRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericUpdateListCommandHandler<Workflow, WorkflowDto>));

                    // GenericUpdateListRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericUpdateListRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(GenericApiResponse<string>),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericUpdateListCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);

                    // Example handling input GenericUpdateRequest:
                    //services.AddScoped(typeof(IRequestHandler<GenericUpdateRequest<WorkflowDto>, GenericApiResponse<string>>),
                    //    typeof(GenericUpdateCommandHandler<Workflow, WorkflowDto>));

                    // GenericUpdateRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericUpdateRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(GenericApiResponse<string>),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericUpdateCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);
                }

                var genericGetControllerType = typeof(GenericGetController<,>).MakeGenericType(entityType, dtoType);
                if (genericGetControllerType.IsAssignableFrom(type))
                {
                    // Example handling input GenericApiResponse<TDto>:
                    //services.AddScoped(typeof(IRequestHandler<GenericApiResponse<WorkflowDto>, ActionResult>), typeof(GenericApiResponseHandler<WorkflowDto>));

                    // GenericApiResponse<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericApiResponse<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(ActionResult),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericApiResponseHandler<>).MakeGenericType(dtoType),
                        mappings, entityType, dtoType, logger);

                    // Example handling input GenericGetRequest<TDto>:
                    //services.AddScoped(typeof(IRequestHandler<GenericGetRequest<WorkflowDto>, GenericApiResponse<WorkflowDto>>),
                    //    typeof(GenericGetCommandHandler<Workflow, WorkflowDto>));

                    // GenericGetRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericGetRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(GenericApiResponse<>).MakeGenericType(dtoType),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericGetCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);
                }

                var genericOnlyGetAllControllerType = typeof(GenericOnlyGetAllController<,>).MakeGenericType(entityType, dtoType);
                if (genericOnlyGetAllControllerType.IsAssignableFrom(type))
                {
                    // Example handling input GenericApiResponse<List<TDto>>:
                    //services.AddScoped(typeof(IRequestHandler<GenericApiResponse<List<WorkflowDto>>, ActionResult>), typeof(GenericApiResponseHandler<List<WorkflowDto>>));

                    // GenericApiResponse<List<TDto>>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericApiResponse<>).MakeGenericType(typeof(List<>).MakeGenericType(dtoType)),
                        (entityType, dtoType) => typeof(ActionResult),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) =>
                            typeof(GenericApiResponseHandler<>).MakeGenericType(typeof(List<>).MakeGenericType(dtoType)),
                        mappings, entityType, dtoType, logger);

                    // Example handling input GenericGetListRequest<TDto>:
                    //services.AddScoped(typeof(IRequestHandler<GenericGetListRequest<WorkflowDto>, GenericApiResponse<List<WorkflowDto>>>),
                    //    typeof(GenericGetListCommandHandler<Workflow, WorkflowDto>));

                    // GenericGetListRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericGetListRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(GenericApiResponse<>).MakeGenericType(typeof(List<>).MakeGenericType(dtoType)),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericGetListCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);

                    // Example handling input GenericGetQueryableRequest<TDto>:
                    //services.AddScoped(typeof(IRequestHandler<GenericGetQueryableRequest<WorkflowDto>, IQueryable<WorkflowDto>>),
                    //    typeof(GenericGetQueryableCommandHandler<Workflow, WorkflowDto>));

                    // GenericGetQueryableRequest<TDto>:
                    MediatorHelperSetupTypeWrapInTryCatch(
                        (entityType, dtoType) => typeof(GenericGetQueryableRequest<>).MakeGenericType(dtoType),
                        (entityType, dtoType) => typeof(IQueryable<>).MakeGenericType(dtoType),
                        (requestType, responseType, entityType, dtoType) => typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType),
                        (requestType, responseType, entityType, dtoType) => typeof(GenericGetQueryableCommandHandler<,>).MakeGenericType(entityType, dtoType),
                        mappings, entityType, dtoType, logger);
                }
            }

            return mappings;
        }

        private static List<Type> GetControllerTypes()
        {
            return typeof(Program)
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && t.IsPublic
                    && !t.IsAbstract
                    && (t.IsAssignableTo(typeof(ApiController)) || t.IsAssignableTo(typeof(ControllerBase)))
                    && t.BaseType?.GetGenericArguments()?.Length == 2).ToList();
        }

        /// <summary>
        /// The main function creating interface-implementation mapping and adding them to the <paramref name="mappings"/>
        /// /// </summary>
        private static void MediatorHelperSetupTypeWrapInTryCatch(
            // (entityType, dtoType) => typeof(...).MakeGenericType(...)
            Func<Type, Type, Type?> requestTypeFunc,

            // (entityType, dtoType) => typeof(...).MakeGenericType(...)
            Func<Type, Type, Type?> responseTypeFunc,

            // (requestType, responseType, entityType, dtoType) => typeof(...).MakeGenericType(...)
            Func<Type?, Type?, Type, Type, Type?> interfaceTypeFunc,

            // (requestType, responseType, entityType, dtoType) => typeof(...).MakeGenericType(...)
            Func<Type?, Type?, Type, Type, Type?> implementedTypeFunc,

            List<InterfaceImplementationMapper> mappings,
            Type entityType,
            Type dtoType,
            ILogger logger)
        {
            Type? requestType = null;
            Type? responseType = null;
            Type? interfaceType = null;
            Type? implementedType = null;

            try
            {
                requestType = requestTypeFunc?.Invoke(entityType, dtoType);
                responseType = responseTypeFunc?.Invoke(entityType, dtoType);
                interfaceType = interfaceTypeFunc?.Invoke(requestType, responseType, entityType, dtoType);
                implementedType = implementedTypeFunc?.Invoke(requestType, responseType, entityType, dtoType);

                var genericApiResponseHandlerMapper = new InterfaceImplementationMapper(interfaceType, implementedType);
                if (!interfaceType.IsAssignableFrom(implementedType))
                    throw new InvalidOperationException($"Implementation {implementedType} doesn't implement the interface {interfaceType}");
                if (!mappings.Contains(genericApiResponseHandlerMapper))
                    mappings.Add(genericApiResponseHandlerMapper);
            }
            catch (Exception ex)
            {
                logger.Error(ex,
                    LogConstants.SerilogTemplateExceptionMediatorHelper,
                    nameof(MediatorHelperSetupTypeWrapInTryCatch),
                    requestType?.FullName,
                    responseType?.FullName,
                    interfaceType?.FullName,
                    implementedType?.FullName,
                    JsonConvert.SerializeObject(mappings));
            }
        }
    }
}