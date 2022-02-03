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
    public class TestWorkflowStateInputCodeTypeCommandHandlers : GenericCommandHandlerTest
    {
        [TestMethod]
        public async Task GenericGetListCommandHandler_WorkflowStateInputCodeType_WorkflowStateInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowStateInputCodeType_includePathList = new List<string> { nameof(WorkflowStateInputCodeType.State), nameof(WorkflowStateInputCodeType.InputCodeType) };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDto = new WorkflowStateInputCodeTypeDto(uniqueId) { StateCode = workflowStateDto.Code, InputCodeTypeCode = workflowInputCodeTypeDto.Code };

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                workflowStateInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericGetListCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeType_includePathList,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
            Assert.AreEqual(response.Payload?.Count, 1);
        }

        [TestMethod]
        public async Task GenericGetCommandHandler_WorkflowStateInputCodeType_WorkflowStateInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowStateInputCodeType_includePathList = new List<string> { nameof(WorkflowStateInputCodeType.State), nameof(WorkflowStateInputCodeType.InputCodeType) };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDto = new WorkflowStateInputCodeTypeDto(uniqueId) { StateCode = workflowStateDto.Code, InputCodeTypeCode = workflowInputCodeTypeDto.Code };

            var workflowStateInputCodeTypeCode = workflowStateInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowStateInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                workflowStateInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericGetCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeType_includePathList,
                workflowStateInputCodeTypeCode,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericCreateCommandHandler_WorkflowStateInputCodeType_WorkflowStateInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDto = new WorkflowStateInputCodeTypeDto(uniqueId) { StateCode = workflowStateDto.Code, InputCodeTypeCode = workflowInputCodeTypeDto.Code };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericCreateCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                user,
                workflowStateInputCodeType_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericCreateListCommandHandler_WorkflowStateInputCodeType_WorkflowStateInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowTypeCode = workflowTypeDto.Code;
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeCode };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDto = new WorkflowStateInputCodeTypeDto(uniqueId) { StateCode = workflowStateDto.Code, InputCodeTypeCode = workflowInputCodeTypeDto.Code };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);

            var workflowStateInputCodeTypeCollection = new Collection<WorkflowStateInputCodeTypeDto> { workflowStateInputCodeTypeDto };

            // 2. Act:
            var response = await GenericCreateListCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeCollection,
                user,
                workflowStateInputCodeType_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericUpdateCommandHandler_WorkflowStateInputCodeType_WorkflowStateInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowStateInputCodeType_includePathList = new List<string> { nameof(WorkflowStateInputCodeType.State), nameof(WorkflowStateInputCodeType.InputCodeType) };
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoOld = new WorkflowStateDto(uniqueId, "Old") { WorkflowCode = workflowDto.Code };
            var workflowStateDtoNew = new WorkflowStateDto(uniqueId, "New") { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDtoOld = new WorkflowInputCodeTypeDto(uniqueId, "Old") { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDtoNew = new WorkflowInputCodeTypeDto(uniqueId, "New") { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDto = new WorkflowStateInputCodeTypeDto(uniqueId) { StateCode = workflowStateDtoOld.Code, InputCodeTypeCode = workflowInputCodeTypeDtoOld.Code };

            var workflowStateInputCodeTypeCode = workflowStateInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowStateInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDtoOld,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDtoNew,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDtoOld,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDtoNew,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                workflowStateInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // Update item
            workflowStateInputCodeTypeDto.StateCode = workflowStateDtoNew.Code;
            workflowStateInputCodeTypeDto.InputCodeTypeCode = workflowInputCodeTypeDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                user,
                workflowStateInputCodeType_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeType_includePathList,
                workflowStateInputCodeTypeCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericUpdateListCommandHandler_WorkflowStateInputCodeType_WorkflowStateInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowStateInputCodeType_includePathList = new List<string> { nameof(WorkflowStateInputCodeType.State), nameof(WorkflowStateInputCodeType.InputCodeType) };
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoOld = new WorkflowStateDto(uniqueId, "Old") { WorkflowCode = workflowDto.Code };
            var workflowStateDtoNew = new WorkflowStateDto(uniqueId, "New") { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDtoOld = new WorkflowInputCodeTypeDto(uniqueId, "Old") { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDtoNew = new WorkflowInputCodeTypeDto(uniqueId, "New") { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDto = new WorkflowStateInputCodeTypeDto(uniqueId) { StateCode = workflowStateDtoOld.Code, InputCodeTypeCode = workflowInputCodeTypeDtoOld.Code };

            var workflowStateInputCodeTypeCode = workflowStateInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowStateInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDtoOld,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDtoNew,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDtoOld,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDtoNew,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                workflowStateInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // Update item
            workflowStateInputCodeTypeDto.StateCode = workflowStateDtoNew.Code;
            workflowStateInputCodeTypeDto.InputCodeTypeCode = workflowInputCodeTypeDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateListCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                user,
                workflowStateInputCodeType_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeType_includePathList,
                workflowStateInputCodeTypeCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericDeleteCommandHandler_WorkflowStateInputCodeType_WorkflowStateInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDto = new WorkflowStateInputCodeTypeDto(uniqueId) { StateCode = workflowStateDto.Code, InputCodeTypeCode = workflowInputCodeTypeDto.Code };

            var workflowStateInputCodeTypeCode = workflowStateInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowStateInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);
            var user = GetDefaultUser();

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                workflowStateInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericDeleteCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeCode,
                user,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);
        }

        [TestMethod]
        public async Task GenericDeleteListCommandHandler_WorkflowStateInputCodeType_WorkflowStateInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDto = new WorkflowStateDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDto = new WorkflowStateInputCodeTypeDto(uniqueId) { StateCode = workflowStateDto.Code, InputCodeTypeCode = workflowInputCodeTypeDto.Code };

            var workflowStateInputCodeTypeCode = workflowStateInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowStateInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);
            var user = GetDefaultUser();

            // Prepare the database
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(
                workflowTypeDto,
                workflowType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(
                workflowDto,
                workflow_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDto,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDto,
                workflowStateInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            var codes = new Collection<string> { workflowStateInputCodeTypeCode };

            // 2. Act:
            var response = await GenericDeleteListCommandHandlerExecute<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
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