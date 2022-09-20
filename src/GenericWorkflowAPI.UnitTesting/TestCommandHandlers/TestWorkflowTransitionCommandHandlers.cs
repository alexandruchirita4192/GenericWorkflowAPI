using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestWorkflowTransitionCommandHandlers : GenericCommandHandlerTest
    {
        [TestMethod]
        public async Task GenericGetListCommandHandler_WorkflowTransition_WorkflowTransitionDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowTransition_includePathList = new List<string> {
                nameof(WorkflowTransition.Workflow),
                nameof(WorkflowTransition.CurrentState),
                nameof(WorkflowTransition.NextState),
                nameof(WorkflowTransition.Role)
            };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrent = new WorkflowStateDto(uniqueId, "Current") { WorkflowCode = workflowDto.Code };
            var workflowStateDtoNext = new WorkflowStateDto(uniqueId, "Next") { WorkflowCode = workflowDto.Code };
            var identityRoleDto = new IdentityRole("TestRole");
            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDto.Code,
                CurrentStateCode = workflowStateDtoCurrent.Code,
                NextStateCode = workflowStateDtoNext.Code,
                RoleCode = identityRoleDto.Code
            };

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
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
                workflowStateDtoCurrent,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNext,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDto,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                workflowTransition_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericGetListCommandHandlerExecute<WorkflowTransition, WorkflowTransitionDto>(
                workflowTransition_includePathList,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
            Assert.AreEqual(response.Payload?.Count, 1);
        }

        [TestMethod]
        public async Task GenericGetCommandHandler_WorkflowTransition_WorkflowTransitionDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowTransition_includePathList = new List<string> {
                nameof(WorkflowTransition.Workflow),
                nameof(WorkflowTransition.CurrentState),
                nameof(WorkflowTransition.NextState),
                nameof(WorkflowTransition.Role)
            };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrent = new WorkflowStateDto(uniqueId, "Current") { WorkflowCode = workflowDto.Code };
            var workflowStateDtoNext = new WorkflowStateDto(uniqueId, "Next") { WorkflowCode = workflowDto.Code };
            var identityRoleDto = new IdentityRole("TestRole");
            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDto.Code,
                CurrentStateCode = workflowStateDtoCurrent.Code,
                NextStateCode = workflowStateDtoNext.Code,
                RoleCode = identityRoleDto.Code
            };

            var workflowTransitionCode = workflowTransitionDto.Code;
            Assert.IsNotNull(workflowTransitionCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
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
                workflowStateDtoCurrent,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNext,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDto,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                workflowTransition_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericGetCommandHandlerExecute<WorkflowTransition, WorkflowTransitionDto>(
                workflowTransition_includePathList,
                workflowTransitionCode,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericCreateCommandHandler_WorkflowTransition_WorkflowTransitionDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrent = new WorkflowStateDto(uniqueId, "Current") { WorkflowCode = workflowDto.Code };
            var workflowStateDtoNext = new WorkflowStateDto(uniqueId, "Next") { WorkflowCode = workflowDto.Code };
            var identityRoleDto = new IdentityRole("TestRole");
            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDto.Code,
                CurrentStateCode = workflowStateDtoCurrent.Code,
                NextStateCode = workflowStateDtoNext.Code,
                RoleCode = identityRoleDto.Code
            };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
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
                workflowStateDtoCurrent,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNext,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDto,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericCreateCommandHandlerExecute<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                user,
                workflowTransition_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericCreateListCommandHandler_WorkflowTransition_WorkflowTransitionDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrent = new WorkflowStateDto(uniqueId, "Current") { WorkflowCode = workflowDto.Code };
            var workflowStateDtoNext = new WorkflowStateDto(uniqueId, "Next") { WorkflowCode = workflowDto.Code };
            var identityRoleDto = new IdentityRole("TestRole");
            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDto.Code,
                CurrentStateCode = workflowStateDtoCurrent.Code,
                NextStateCode = workflowStateDtoNext.Code,
                RoleCode = identityRoleDto.Code
            };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
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
                workflowStateDtoCurrent,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNext,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDto,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);

            var workflowTransitionCollection = new Collection<WorkflowTransitionDto> { workflowTransitionDto };

            // 2. Act:
            var response = await GenericCreateListCommandHandlerExecute<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionCollection,
                user,
                workflowTransition_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericUpdateCommandHandler_WorkflowTransition_WorkflowTransitionDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowTransition_includePathList = new List<string> {
                nameof(WorkflowTransition.Workflow),
                nameof(WorkflowTransition.CurrentState),
                nameof(WorkflowTransition.NextState),
                nameof(WorkflowTransition.Role)
            };
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDtoOld = new WorkflowDto(uniqueId, "Old") { TypeCode = workflowTypeDto.Code };
            var workflowDtoNew = new WorkflowDto(uniqueId, "New") { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrentOld = new WorkflowStateDto(uniqueId, "CurrentOld") { WorkflowCode = workflowDtoOld.Code };
            var workflowStateDtoCurrentNew = new WorkflowStateDto(uniqueId, "CurrentNew") { WorkflowCode = workflowDtoNew.Code };
            var workflowStateDtoNextOld = new WorkflowStateDto(uniqueId, "NextOld") { WorkflowCode = workflowDtoOld.Code };
            var workflowStateDtoNextNew = new WorkflowStateDto(uniqueId, "NextNew") { WorkflowCode = workflowDtoNew.Code };
            var identityRoleDtoOld = new IdentityRole("TestRoleOld");
            var identityRoleDtoNew = new IdentityRole("TestRoleNew");
            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDtoOld.Code,
                CurrentStateCode = workflowStateDtoCurrentOld.Code,
                NextStateCode = workflowStateDtoNextOld.Code,
                RoleCode = identityRoleDtoOld.Code
            };

            var workflowTransitionCode = workflowTransitionDto.Code;
            Assert.IsNotNull(workflowTransitionCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
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
                workflowStateDtoCurrentOld,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoCurrentNew,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNextOld,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNextNew,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDtoOld,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDtoNew,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                workflowTransition_entityServiceExtraTypes,
                applicationDbContext);

            // Update item
            workflowTransitionDto.WorkflowCode = workflowDtoNew.Code;
            workflowTransitionDto.CurrentStateCode = workflowStateDtoCurrentNew.Code;
            workflowTransitionDto.NextStateCode = workflowStateDtoNextNew.Code;
            workflowTransitionDto.RoleCode = identityRoleDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateCommandHandlerExecute<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                user,
                workflowTransition_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<WorkflowTransition, WorkflowTransitionDto>(
                workflowTransition_includePathList,
                workflowTransitionCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericUpdateListCommandHandler_WorkflowTransition_WorkflowTransitionDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowTransition_includePathList = new List<string> {
                nameof(WorkflowTransition.Workflow),
                nameof(WorkflowTransition.CurrentState),
                nameof(WorkflowTransition.NextState),
                nameof(WorkflowTransition.Role)
            };
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDtoOld = new WorkflowDto(uniqueId, "Old") { TypeCode = workflowTypeDto.Code };
            var workflowDtoNew = new WorkflowDto(uniqueId, "New") { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrentOld = new WorkflowStateDto(uniqueId, "CurrentOld") { WorkflowCode = workflowDtoOld.Code };
            var workflowStateDtoCurrentNew = new WorkflowStateDto(uniqueId, "CurrentNew") { WorkflowCode = workflowDtoNew.Code };
            var workflowStateDtoNextOld = new WorkflowStateDto(uniqueId, "NextOld") { WorkflowCode = workflowDtoOld.Code };
            var workflowStateDtoNextNew = new WorkflowStateDto(uniqueId, "NextNew") { WorkflowCode = workflowDtoNew.Code };
            var identityRoleDtoOld = new IdentityRole("TestRoleOld");
            var identityRoleDtoNew = new IdentityRole("TestRoleNew");
            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDtoOld.Code,
                CurrentStateCode = workflowStateDtoCurrentOld.Code,
                NextStateCode = workflowStateDtoNextOld.Code,
                RoleCode = identityRoleDtoOld.Code
            };

            var workflowTransitionCode = workflowTransitionDto.Code;
            Assert.IsNotNull(workflowTransitionCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
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
                workflowStateDtoCurrentOld,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoCurrentNew,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNextOld,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNextNew,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDtoOld,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDtoNew,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                workflowTransition_entityServiceExtraTypes,
                applicationDbContext);

            // Update item
            workflowTransitionDto.WorkflowCode = workflowDtoNew.Code;
            workflowTransitionDto.CurrentStateCode = workflowStateDtoCurrentNew.Code;
            workflowTransitionDto.NextStateCode = workflowStateDtoNextNew.Code;
            workflowTransitionDto.RoleCode = identityRoleDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateListCommandHandlerExecute<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                user,
                workflowTransition_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<WorkflowTransition, WorkflowTransitionDto>(
                workflowTransition_includePathList,
                workflowTransitionCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericDeleteCommandHandler_WorkflowTransition_WorkflowTransitionDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrent = new WorkflowStateDto(uniqueId, "Current") { WorkflowCode = workflowDto.Code };
            var workflowStateDtoNext = new WorkflowStateDto(uniqueId, "Next") { WorkflowCode = workflowDto.Code };
            var identityRoleDto = new IdentityRole("TestRole");
            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDto.Code,
                CurrentStateCode = workflowStateDtoCurrent.Code,
                NextStateCode = workflowStateDtoNext.Code,
                RoleCode = identityRoleDto.Code
            };

            var workflowTransitionCode = workflowTransitionDto.Code;
            Assert.IsNotNull(workflowTransitionCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
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
                workflowStateDtoCurrent,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNext,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, IdentityRole, IdentityRole>(
                identityRoleDto,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                workflowTransition_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericDeleteCommandHandlerExecute<WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionCode,
                user,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);
        }

        [TestMethod]
        public async Task GenericDeleteListCommandHandler_WorkflowTransition_WorkflowTransitionDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowStateDtoCurrent = new WorkflowStateDto(uniqueId, "Current") { WorkflowCode = workflowDto.Code };
            var workflowStateDtoNext = new WorkflowStateDto(uniqueId, "Next") { WorkflowCode = workflowDto.Code };
            var identityRoleDto = new IdentityRole("TestRole");
            var workflowTransitionDto = new WorkflowTransitionDto(uniqueId)
            {
                WorkflowCode = workflowDto.Code,
                CurrentStateCode = workflowStateDtoCurrent.Code,
                NextStateCode = workflowStateDtoNext.Code,
                RoleCode = identityRoleDto.Code
            };

            var workflowTransitionCode = workflowTransitionDto.Code;
            Assert.IsNotNull(workflowTransitionCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowState_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var identityRole_entityServiceExtraTypes = new List<Type>();
            var workflowTransition_entityServiceExtraTypes = new List<Type> { typeof(Workflow), typeof(WorkflowState), typeof(IdentityRole) };
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
                workflowStateDtoCurrent,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowState, WorkflowStateDto>(
                workflowStateDtoNext,
                workflowState_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, 
            IdentityRole, IdentityRole>(
                identityRoleDto,
                identityRole_entityServiceExtraTypes,
                applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowTransition, WorkflowTransitionDto>(
                workflowTransitionDto,
                workflowTransition_entityServiceExtraTypes,
                applicationDbContext);

            var codes = new Collection<string> { workflowTransitionCode };

            // 2. Act:
            var response = await GenericDeleteListCommandHandlerExecute<WorkflowTransition, WorkflowTransitionDto>(
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