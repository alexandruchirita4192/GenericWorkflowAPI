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
            var uniqueId = DateTime.Now.Ticks;
            var workflowTypeCode = $"Code{uniqueId}WorkflowType";
            var workflowTypeDto = new WorkflowTypeDto
            {
                Code = workflowTypeCode,
                Name = $"Name{uniqueId}WorkflowType",
                Description = $"Description{uniqueId}WorkflowType"
            };
            var workflowDto = new WorkflowDto
            {
                Code = $"Code{uniqueId}Workflow",
                Name = $"Name{uniqueId}Workflow",
                Description = $"Description{uniqueId}Workflow",
                TypeCode = workflowTypeCode
            };
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);

            // Prepare database with a workflow type item and a workflow item
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, new List<Type>(), applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            // 2. Act:
            var response = await GenericGetListCommandHandlerExecute<Workflow, WorkflowDto>(includePathList, true, applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericGetCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var includePathList = new List<string> { nameof(Workflow.Type) };
            var uniqueId = DateTime.Now.Ticks;
            var workflowTypeCode = $"Code{uniqueId}WorkflowType";
            var workflowTypeDto = new WorkflowTypeDto
            {
                Code = workflowTypeCode,
                Name = $"Name{uniqueId}WorkflowType",
                Description = $"Description{uniqueId}WorkflowType"
            };
            var workflowCode = $"Code{uniqueId}Workflow";
            var workflowDto = new WorkflowDto
            {
                Code = workflowCode,
                Name = $"Name{uniqueId}Workflow",
                Description = $"Description{uniqueId}Workflow",
                TypeCode = workflowTypeCode
            };
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);

            // Prepare database with a workflow type item and a workflow item
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, new List<Type>(), applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            // 2. Act:
            var response = await GenericGetCommandHandlerExecute<Workflow, WorkflowDto>(includePathList, workflowCode, true, applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericCreateCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;
            var workflowTypeCode = $"Code{uniqueId}WorkflowType";
            var workflowTypeDto = new WorkflowTypeDto
            {
                Code = workflowTypeCode,
                Name = $"Name{uniqueId}WorkflowType",
                Description = $"Description{uniqueId}WorkflowType"
            };
            var workflowDto = new WorkflowDto
            {
                Code = $"Code{uniqueId}Workflow",
                Name = $"Name{uniqueId}Workflow",
                Description = $"Description{uniqueId}Workflow",
                TypeCode = workflowTypeCode
            };
            var user = GetDefaultUser();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);

            // Prepare the databse with a workflow type item (it's code is required)
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, new List<Type>(), applicationDbContext);

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
            var workflowTypeCode = $"Code{uniqueId}WorkflowType";
            var workflowTypeDto = new WorkflowTypeDto
            {
                Code = workflowTypeCode,
                Name = $"Name{uniqueId}WorkflowType",
                Description = $"Description{uniqueId}WorkflowType"
            };
            var workflowDto = new WorkflowDto
            {
                Code = $"Code{uniqueId}Workflow",
                Name = $"Name{uniqueId}Workflow",
                Description = $"Description{uniqueId}Workflow",
                TypeCode = workflowTypeCode
            };
            var user = GetDefaultUser();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);

            // Prepare the databse with a workflow type item (it's code is required)
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, new List<Type>(), applicationDbContext);

            // 2. Act:
            var response = await GenericCreateListCommandHandlerExecute<Workflow, WorkflowDto>(
                new Collection<WorkflowDto> { workflowDto },
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
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();
            var workflowTypeCodeOld = $"Code{uniqueId}WorkflowTypeOld";
            var workflowTypeCodeNew = $"Code{uniqueId}WorkflowTypeNew";
            var workflowTypeDto = new WorkflowTypeDto
            {
                Code = workflowTypeCodeOld,
                Name = $"Name{uniqueId}WorkflowTypeOld",
                Description = $"Description{uniqueId}WorkflowTypeOld"
            };
            var workflowTypeDto2 = new WorkflowTypeDto
            {
                Code = workflowTypeCodeNew,
                Name = $"Name{uniqueId}WorkflowTypeNew",
                Description = $"Description{uniqueId}WorkflowTypeNew"
            };
            var workflowCode = $"Code{uniqueId}Workflow";
            var workflowDto = new WorkflowDto
            {
                Code = workflowCode,
                Name = $"Name{uniqueId}Workflow",
                Description = $"Description{uniqueId}Workflow",
                TypeCode = workflowTypeCodeOld
            };
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);
            var includePathList_Workflow = new List<string> { nameof(Workflow.Type) };

            // Prepare database with a workflow type item and a workflow item
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, new List<Type>(), applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto2, new List<Type>(), applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            workflowDto.Name += "Updated";
            workflowDto.Description += "Updated";
            workflowDto.TypeCode = workflowTypeCodeNew;

            // 2. Act:
            var response = await GenericUpdateCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowDto,
                user,
                workflow_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print the workflow item
            var assertResponse = await GenericGetCommandHandlerExecute<Workflow, WorkflowDto>(includePathList_Workflow, workflowCode, true, applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericUpdateListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();
            var workflowTypeCodeOld = $"Code{uniqueId}WorkflowTypeOld";
            var workflowTypeCodeNew = $"Code{uniqueId}WorkflowTypeNew";
            var workflowTypeDto = new WorkflowTypeDto
            {
                Code = workflowTypeCodeOld,
                Name = $"Name{uniqueId}WorkflowTypeOld",
                Description = $"Description{uniqueId}WorkflowTypeOld"
            };
            var workflowTypeDto2 = new WorkflowTypeDto
            {
                Code = workflowTypeCodeNew,
                Name = $"Name{uniqueId}WorkflowTypeNew",
                Description = $"Description{uniqueId}WorkflowTypeNew"
            };
            var workflowCode = $"Code{uniqueId}Workflow";
            var workflowDto = new WorkflowDto
            {
                Code = workflowCode,
                Name = $"Name{uniqueId}Workflow",
                Description = $"Description{uniqueId}Workflow",
                TypeCode = workflowTypeCodeOld
            };
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);
            var includePathList_Workflow = new List<string> { nameof(Workflow.Type) };

            // Prepare database with a workflow type item and a workflow item
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, new List<Type>(), applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto2, new List<Type>(), applicationDbContext);
            await GenericCreateCommandHandler_InMemory_WithSelfTest<Workflow, WorkflowDto>(workflowDto, workflow_entityServiceExtraTypes, applicationDbContext);

            workflowDto.Name += "Updated";
            workflowDto.Description += "Updated";
            workflowDto.TypeCode = workflowTypeCodeNew;

            // 2. Act:
            var response = await GenericUpdateListCommandHandlerExecute<Workflow, WorkflowDto>(
                workflowDto,
                user,
                workflow_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print the workflow item
            var assertResponse = await GenericGetCommandHandlerExecute<Workflow, WorkflowDto>(includePathList_Workflow, workflowCode, true, applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericDeleteCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;
            var workflowTypeCode = $"Code{uniqueId}WorkflowType";
            var workflowTypeDto = new WorkflowTypeDto
            {
                Code = workflowTypeCode,
                Name = $"Name{uniqueId}WorkflowType",
                Description = $"Description{uniqueId}WorkflowType"
            };
            var workflowCode = $"Code{uniqueId}Workflow";
            var workflowDto = new WorkflowDto
            {
                Code = workflowCode,
                Name = $"Name{uniqueId}Workflow",
                Description = $"Description{uniqueId}Workflow",
                TypeCode = workflowTypeCode
            };
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);
            var user = GetDefaultUser();

            // Prepare database with a workflow type item and a workflow item
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, new List<Type>(), applicationDbContext);
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
            var workflowTypeCode = $"Code{uniqueId}WorkflowType";
            var workflowTypeDto = new WorkflowTypeDto
            {
                Code = workflowTypeCode,
                Name = $"Name{uniqueId}WorkflowType",
                Description = $"Description{uniqueId}WorkflowType"
            };
            var workflowCode = $"Code{uniqueId}Workflow";
            var workflowDto = new WorkflowDto
            {
                Code = workflowCode,
                Name = $"Name{uniqueId}Workflow",
                Description = $"Description{uniqueId}Workflow",
                TypeCode = workflowTypeCode
            };
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var applicationDbContext = GetSqlServerDbContext(isInMemory: true);
            var user = GetDefaultUser();
            var codes = new Collection<string> { workflowCode };

            // Prepare database with a workflow type item and a workflow item
            await GenericCreateCommandHandler_InMemory_WithSelfTest<WorkflowType, WorkflowTypeDto>(workflowTypeDto, new List<Type>(), applicationDbContext);
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