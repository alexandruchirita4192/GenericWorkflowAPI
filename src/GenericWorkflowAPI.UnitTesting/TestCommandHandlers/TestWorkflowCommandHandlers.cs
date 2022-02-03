using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestWorkflowCommandHandlers : GenericCommandHandlerTest
    {
        [TestMethod]
        public async Task GenericGetListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflow_includePathList = new List<string> { nameof(Workflow.Type) };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, workflowType_entityServiceExtraTypes, applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            // 2. Act:
            var response = await GenericGetListCommandHandlerExecute<Workflow, WorkflowDto>(workflow_includePathList, true, applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
            Assert.AreEqual(response.Payload?.Count, 1);
        }

        [TestMethod]
        public async Task GenericGetCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflow_includePathList = new List<string> { nameof(Workflow.Type) };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowCode = workflowDto.Code;
            Assert.IsNotNull(workflowCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, workflowType_entityServiceExtraTypes, applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            // 2. Act:
            var response = await GenericGetCommandHandlerExecute<Workflow, WorkflowDto>(workflow_includePathList, workflowCode, true, applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericCreateCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, workflowType_entityServiceExtraTypes, applicationDbContext);

            // 2. Act:
            var response = await GenericCreateCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowDto,
                user,
                workflow_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericCreateListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, workflowType_entityServiceExtraTypes, applicationDbContext);

            var workflowCollection = new Collection<WorkflowDto> { workflowDto };

            // 2. Act:
            var response = await GenericCreateListCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowCollection,
                user,
                workflow_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericUpdateCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflow_includePathList = new List<string> { nameof(Workflow.Type) };
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDtoOld = new WorkflowTypeDto(uniqueId, "Old");
            var workflowTypeDtoNew = new WorkflowTypeDto(uniqueId, "New");
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDtoOld.Code };
            var workflowCode = workflowDto.Code;
            Assert.IsNotNull(workflowCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDtoOld, workflowType_entityServiceExtraTypes, applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDtoNew, workflowType_entityServiceExtraTypes, applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            // Update item
            workflowDto.Name += "Updated";
            workflowDto.Description += "Updated";
            workflowDto.TypeCode = workflowTypeDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowDto,
                user,
                workflow_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<Workflow, WorkflowDto>(workflow_includePathList, workflowCode, true, applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericUpdateListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflow_includePathList = new List<string> { nameof(Workflow.Type) };
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDtoOld = new WorkflowTypeDto(uniqueId, "Old");
            var workflowTypeDtoNew = new WorkflowTypeDto(uniqueId, "New");
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDtoOld.Code };
            var workflowCode = workflowDto.Code;
            Assert.IsNotNull(workflowCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDtoOld, workflowType_entityServiceExtraTypes, applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDtoNew, workflowType_entityServiceExtraTypes, applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            // Update item
            workflowDto.Name += "Updated";
            workflowDto.Description += "Updated";
            workflowDto.TypeCode = workflowTypeDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateListCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowDto,
                user,
                workflow_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<Workflow, WorkflowDto>(workflow_includePathList, workflowCode, true, applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericDeleteCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowCode = workflowDto.Code;
            Assert.IsNotNull(workflowCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);
            var user = GetDefaultUser();

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, workflowType_entityServiceExtraTypes, applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            // 2. Act:
            var response = await GenericDeleteCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowCode,
                user,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);
        }

        [TestMethod]
        public async Task GenericDeleteListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowCode = workflowDto.Code;
            Assert.IsNotNull(workflowCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);
            var user = GetDefaultUser();

            var codes = new Collection<string> { workflowCode };

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, workflowType_entityServiceExtraTypes, applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            // 2. Act:
            var response = await GenericDeleteListCommandHandlerExecute<Workflow, WorkflowDto>(
                codes,
                user,
                true,
                applicationDbContext
                );

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);
        }
    }
}