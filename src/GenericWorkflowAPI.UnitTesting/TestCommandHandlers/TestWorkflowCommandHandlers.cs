using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain;
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

        [TestMethod]
        public async Task ExecuteWorkflow_InitiateWorkflowInstanceAndExecuteTransition_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowInstance_includePathList = new List<string> {
                      nameof(WorkflowInstance.Workflow),
                      nameof(WorkflowInstance.CurrentState)
            };
            var workflowInstanceInputCode_includePathList = new List<string>
            {
                nameof(WorkflowInstanceInputCode.Instance),
                nameof(WorkflowInstanceInputCode.Type)
            };
            var workflowInstanceHistory_includePathList = new List<string>
            {
                nameof(WorkflowInstanceHistory.Instance),
                nameof(WorkflowInstanceHistory.CurrentState),
                nameof(WorkflowInstanceHistory.NextState),
            };
            var workflowInstanceHistoryInputCode_includePathList = new List<string>
            {
                nameof(WorkflowInstanceHistoryInputCode.History),
                nameof(WorkflowInstanceHistoryInputCode.InputCodeType),
            };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrent = new WorkflowStateDto(uniqueId, "Current") { WorkflowCode = workflowDto.Code, IsFirstState = true };
            var workflowStateDtoNext = new WorkflowStateDto(uniqueId, "Next") { WorkflowCode = workflowDto.Code };
            var role = "TestRole";
            var identityRole = new IdentityRole(role) { Id = 1 };

            var transitionRoleCode = identityRole.Code;
            Assert.IsNotNull(transitionRoleCode);

            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDto.Code,
                CurrentStateCode = workflowStateDtoCurrent.Code,
                NextStateCode = workflowStateDtoNext.Code,
                RoleCode = transitionRoleCode
            };
            var workflowInputCodeTypeDtoCurrent = new WorkflowInputCodeTypeDto(uniqueId, "Current") { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeDtoNext = new WorkflowInputCodeTypeDto(uniqueId, "Next") { WorkflowCode = workflowDto.Code };
            var workflowStateInputCodeTypeDtoCurrent = new WorkflowStateInputCodeTypeDto(uniqueId, "Current")
            {
                StateCode = workflowStateDtoCurrent.Code,
                InputCodeTypeCode = workflowInputCodeTypeDtoCurrent.Code
            };
            var workflowStateInputCodeTypeDtoNext = new WorkflowStateInputCodeTypeDto(uniqueId, "Next")
            {
                StateCode = workflowStateDtoNext.Code,
                InputCodeTypeCode = workflowInputCodeTypeDtoNext.Code
            };

            var workflowCode = workflowDto.Code;
            Assert.IsNotNull(workflowCode);

            var workflowInstanceCode = new WorkflowInstanceDto(uniqueId).Code;
            Assert.IsNotNull(workflowInstanceCode);

            var workflowInputCodeTypeCurrentCode = workflowInputCodeTypeDtoCurrent.Code;
            Assert.IsNotNull(workflowInputCodeTypeCurrentCode);

            var workflowInputCodeTypeNextCode = workflowInputCodeTypeDtoNext.Code;
            Assert.IsNotNull(workflowInputCodeTypeNextCode);

            var workflowInputCodeTypeXvalue_InitiateWorkflowInstance = new Dictionary<string, string>
            {
                { workflowInputCodeTypeCurrentCode, $"InputData{uniqueId}Current" }
            };
            var workflowInputCodeTypeXvalue_ExecuteTransition = new Dictionary<string, string>
            {
                { workflowInputCodeTypeNextCode, $"InputData{uniqueId}Next" }
            };

            var roles_InitializeWorkflowInstance = new List<string>();
            var roles_ExecuteTransition = new List<string> { transitionRoleCode };

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var workflowStateInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(WorkflowState), typeof(WorkflowInputCodeType) };
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);

            // Add the user to the database
            await applicationDbContext.AddAsync(GetDefaultUser());
            applicationDbContext.SaveChanges();

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
                workflowStateDtoCurrent,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowState, WorkflowStateDto>(
                workflowStateDtoNext,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<IdentityRole, IdentityRole>(
                identityRole,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                workflowTransition_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDtoCurrent,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDtoNext,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDtoCurrent,
                workflowStateInputCodeType_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>(
                workflowStateInputCodeTypeDtoNext,
                workflowStateInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            // Initiate workflow instance
            var response_InitiateWorkflowInstance = await GenericExecuteWorkflowCommandHandlerExecute(
                workflowCode,
                workflowInstanceCode,
                workflowInputCodeTypeXvalue_InitiateWorkflowInstance,
                roles_InitializeWorkflowInstance,
                true,
                applicationDbContext);

            // Execute workflow transition
            var response_ExecuteWorkflowTransition = await GenericExecuteWorkflowCommandHandlerExecute(
                workflowCode,
                workflowInstanceCode,
                workflowInputCodeTypeXvalue_ExecuteTransition,
                roles_ExecuteTransition,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response_InitiateWorkflowInstance, HttpStatusCode.OK, false);
            AssertGenericApiResponse(response_ExecuteWorkflowTransition, HttpStatusCode.OK, false);

            // Print resulted items
            var response_WorkflowInstance = await GenericGetCommandHandlerExecute<WorkflowInstance, WorkflowInstanceDto>(
                workflowInstance_includePathList,
                workflowInstanceCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(response_WorkflowInstance, HttpStatusCode.OK);
             
            var response_InstanceInputCodes = await GenericGetListCommandHandlerExecute<WorkflowInstanceInputCode, WorkflowInstanceInputCodeDto>(
                workflowInstanceInputCode_includePathList,
                true,
                applicationDbContext);
            AssertGenericApiResponse(response_InstanceInputCodes, HttpStatusCode.OK);
            Assert.AreEqual(response_InstanceInputCodes.Payload?.Count, 2);

            var response_Histories = await GenericGetListCommandHandlerExecute<WorkflowInstanceHistory, WorkflowInstanceHistoryDto>(
                workflowInstanceHistory_includePathList,
                true,
                applicationDbContext);
            AssertGenericApiResponse(response_Histories, HttpStatusCode.OK);
            Assert.AreEqual(response_Histories.Payload?.Count, 2);

            var response_HistorieInputCodes = await GenericGetListCommandHandlerExecute<WorkflowInstanceHistoryInputCode, WorkflowInstanceHistoryInputCodeDto>(
                workflowInstanceHistoryInputCode_includePathList,
                true,
                applicationDbContext);
            AssertGenericApiResponse(response_HistorieInputCodes, HttpStatusCode.OK);
            Assert.AreEqual(response_HistorieInputCodes.Payload?.Count, 2);
        }
    }
}