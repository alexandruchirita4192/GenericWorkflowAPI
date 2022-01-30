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
    public class TestCommandHandlers : GenericCommandHandlerTest
    {
        [TestMethod]
        public async Task GenericGetListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var includePathList = new List<string> { nameof(Workflow.Type) };

            // 2. Act:
            var response = await GenericGetListCommandHandlerExecute<Workflow, WorkflowDto>(includePathList, false);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericGetCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var includePathList = new List<string> { nameof(Workflow.Type) };
            var code = "Code637777973942605064"; // TODO: This shouldn't be hard-coded

            // 2. Act:
            var response = await GenericGetCommandHandlerExecute<Workflow, WorkflowDto>(includePathList, code, false);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericCreateCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;
            var workflowDto = new WorkflowDto
            {
                Code = $"Code{uniqueId}",
                Name = $"Name{uniqueId}",
                Description = $"Description{uniqueId}",
                TypeCode = "TestCode" // TODO: This shouldn't be hard-coded
            };
            var user = GetDefaultUser();
            var entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };

            // 2. Act:
            var response = await GenericCreateCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowDto,
                user,
                entityServiceExtraTypes,
                false);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericCreateListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;
            var workflowDto = new WorkflowDto
            {
                Code = $"Code{uniqueId}",
                Name = $"Name{uniqueId}",
                Description = $"Description{uniqueId}",
                TypeCode = "TestCode" // TODO: This shouldn't be hard-coded
            };
            var user = GetDefaultUser();
            var entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };

            // 2. Act:
            var response = await GenericCreateListCommandHandlerExecute<Workflow, WorkflowDto>(
                new Collection<WorkflowDto> { workflowDto },
                user,
                entityServiceExtraTypes,
                false);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericUpdateCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;
            var workflowDto = new WorkflowDto
            {
                Code = $"Code637777973942605064",
                Name = $"Name{uniqueId}",
                Description = $"Description{uniqueId}",
                TypeCode = "TestCode" // TODO: This shouldn't be hard-coded
            };
            var user = GetDefaultUser();
            var entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };

            // 2. Act:
            var response = await GenericUpdateCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowDto,
                user,
                entityServiceExtraTypes,
                false);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericUpdateListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            var uniqueId = DateTime.Now.Ticks;
            var workflowDto = new WorkflowDto
            {
                Code = $"Code637777973942605064",
                Name = $"Name{uniqueId}",
                Description = $"Description{uniqueId}",
                TypeCode = "TestCode" // TODO: This shouldn't be hard-coded
            };
            var user = GetDefaultUser();
            var entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };

            // 2. Act:
            var response = await GenericUpdateListCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowDto,
                user,
                entityServiceExtraTypes,
                false);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericDeleteCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var code = "Code637777971885190442"; // TODO: This shouldn't be hard-coded
            var user = GetDefaultUser();

            // 2. Act:
            var response = await GenericDeleteCommandHandlerExecute<Workflow, WorkflowDto>(
                code,
                user,
                false);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericDeleteListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var codes = new Collection<string> { "Code637777967760777833" }; // TODO: This shouldn't be hard-coded
            var user = GetDefaultUser();

            // 2. Act:
            var response = await GenericDeleteListCommandHandlerExecute<Workflow, WorkflowDto>(
                codes,
                user,
                false
                );

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }
    }
}