using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestWorkflowStateCommandHandlers : GenericCommandHandlerTest
    {
        [TestMethod]
        public async Task GenericGetListCommandHandler_WorkflowState_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowState_includePathList = new List<string> { nameof(WorkflowState.Workflow) };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericGetListCommandHandlerExecute<WorkflowState, WorkflowStateDto>(
                workflowState_includePathList,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
            Assert.AreEqual(response.Payload?.Count, 1);
        }

        [TestMethod]
        public async Task GenericGetCommandHandler_WorkflowState_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowState_includePathList = new List<string> { nameof(WorkflowState.Workflow) };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };

            var workflowStateCode = workflowStateDto.Code;
            Assert.IsNotNull(workflowStateCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericGetCommandHandlerExecute<WorkflowState, WorkflowStateDto>(
                workflowState_includePathList,
                workflowStateCode,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericCreateCommandHandler_WorkflowState_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericCreateCommandHandlerExecute<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                user,
                workflowState_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericCreateListCommandHandler_WorkflowState_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowTypeCode = workflowTypeDto.Code;
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeCode };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);

            var workflowInputCodeTypeCollection = new Collection<WorkflowStateDto> { workflowStateDto };

            // 2. Act:
            var response = await GenericCreateListCommandHandlerExecute<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowInputCodeTypeCollection,
                user,
                workflowState_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericUpdateCommandHandler_WorkflowState_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowState_includePathList = new List<string> { nameof(WorkflowState.Workflow) };
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDtoOld = new WorkflowDto(uniqueId, "Old") { TypeCode = workflowTypeDto.Code };
            var workflowDtoNew = new WorkflowDto(uniqueId, "New") { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDtoOld.Code };

            var workflowStateCode = workflowStateDto.Code;
            Assert.IsNotNull(workflowStateCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDtoOld,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDtoNew,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);

            // Update item
            workflowStateDto.Name += "Updated";
            workflowStateDto.Description += "Updated";
            workflowStateDto.WorkflowCode = workflowDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateCommandHandlerExecute<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                user,
                workflowState_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<WorkflowState, WorkflowStateDto>(
                workflowState_includePathList,
                workflowStateCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericUpdateListCommandHandler_WorkflowState_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowState_includePathList = new List<string> { nameof(WorkflowState.Workflow) };
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDtoOld = new WorkflowDto(uniqueId, "Old") { TypeCode = workflowTypeDto.Code };
            var workflowDtoNew = new WorkflowDto(uniqueId, "New") { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDtoOld.Code };

            var workflowInputCodeTypeCode = workflowStateDto.Code;
            Assert.IsNotNull(workflowInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDtoOld,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDtoNew,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);

            // Update item
            workflowStateDto.Name += "Updated";
            workflowStateDto.Description += "Updated";
            workflowStateDto.WorkflowCode = workflowDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateListCommandHandlerExecute<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                user,
                workflowState_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<WorkflowState, WorkflowStateDto>(
                workflowState_includePathList,
                workflowInputCodeTypeCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericDeleteCommandHandler_WorkflowState_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeCode = workflowStateDto.Code;
            Assert.IsNotNull(workflowInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);
            var user = GetDefaultUser();

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericDeleteCommandHandlerExecute<WorkflowState, WorkflowStateDto>(
                workflowInputCodeTypeCode,
                user,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);
        }

        [TestMethod]
        public async Task GenericDeleteListCommandHandler_WorkflowState_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowStateCode = workflowStateDto.Code;
            Assert.IsNotNull(workflowStateCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);
            var user = GetDefaultUser();

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);

            var codes = new Collection<string> { workflowStateCode };

            // 2. Act:
            var response = await GenericDeleteListCommandHandlerExecute<WorkflowState, WorkflowStateDto>(
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