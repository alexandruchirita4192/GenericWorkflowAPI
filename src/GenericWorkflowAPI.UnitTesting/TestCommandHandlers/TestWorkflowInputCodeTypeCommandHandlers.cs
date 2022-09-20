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
    public class TestWorkflowInputCodeTypeCommandHandlers : GenericCommandHandlerTest
    {
        [TestMethod]
        public async Task GenericGetListCommandHandler_WorkflowInputCodeType_WorkflowInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowInputCodeType_includePathList = new List<string> { nameof(WorkflowInputCodeType.Workflow) };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
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
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericGetListCommandHandlerExecute<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeType_includePathList,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
            Assert.AreEqual(response.Payload?.Count, 1);
        }

        [TestMethod]
        public async Task GenericGetCommandHandler_WorkflowInputCodeType_WorkflowInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var workflowInputCodeType_includePathList = new List<string> { nameof(WorkflowInputCodeType.Workflow) };
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };

            var workflowInputCodeTypeCode = workflowInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
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
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericGetCommandHandlerExecute<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeType_includePathList,
                workflowInputCodeTypeCode,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericCreateCommandHandler_WorkflowInputCodeType_WorkflowInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
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
            var response = await GenericCreateCommandHandlerExecute<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                user,
                workflowInputCodeType_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericCreateListCommandHandler_WorkflowInputCodeType_WorkflowInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowTypeCode = workflowTypeDto.Code;
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeCode };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };

            var user = GetDefaultUser();
            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
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

            var workflowInputCodeTypeCollection = new Collection<WorkflowInputCodeTypeDto> { workflowInputCodeTypeDto };

            // 2. Act:
            var response = await GenericCreateListCommandHandlerExecute<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeCollection,
                user,
                workflowInputCodeType_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task GenericUpdateCommandHandler_WorkflowInputCodeType_WorkflowInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDtoOld = new WorkflowDto(uniqueId, "Old") { TypeCode = workflowTypeDto.Code };
            var workflowDtoNew = new WorkflowDto(uniqueId, "New") { TypeCode = workflowTypeDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDtoOld.Code };

            var workflowInputCodeTypeCode = workflowInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);
            var workflowInputCodeType_includePathList = new List<string> { nameof(WorkflowInputCodeType.Workflow) };

            // Prepare database with a workflow type item and a workflow item
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
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // Update item
            workflowInputCodeTypeDto.Name += "Updated";
            workflowInputCodeTypeDto.Description += "Updated";
            workflowInputCodeTypeDto.WorkflowCode = workflowDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateCommandHandlerExecute<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                user,
                workflowInputCodeType_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeType_includePathList,
                workflowInputCodeTypeCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericUpdateListCommandHandler_WorkflowInputCodeType_WorkflowInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;
            var user = GetDefaultUser();

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDtoOld = new WorkflowDto(uniqueId, "Old") { TypeCode = workflowTypeDto.Code };
            var workflowDtoNew = new WorkflowDto(uniqueId, "New") { TypeCode = workflowTypeDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDtoOld.Code };

            var workflowInputCodeTypeCode = workflowInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
            var applicationDbContext = GetSqlServerDbContext(null, true, uniqueId);
            var workflowInputCodeType_includePathList = new List<string> { nameof(WorkflowInputCodeType.Workflow) };

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
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // Update item
            workflowInputCodeTypeDto.Name += "Updated";
            workflowInputCodeTypeDto.Description += "Updated";
            workflowInputCodeTypeDto.WorkflowCode = workflowDtoNew.Code;

            // 2. Act:
            var response = await GenericUpdateListCommandHandlerExecute<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                user,
                workflowInputCodeType_entityServiceExtraTypes,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);

            // Print resulted item
            var assertResponse = await GenericGetCommandHandlerExecute<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeType_includePathList,
                workflowInputCodeTypeCode,
                true,
                applicationDbContext);
            AssertGenericApiResponse(assertResponse, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GenericDeleteCommandHandler_WorkflowInputCodeType_WorkflowInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeCode = workflowInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
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
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            // 2. Act:
            var response = await GenericDeleteCommandHandlerExecute<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeCode,
                user,
                true,
                applicationDbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.OK, false);
        }

        [TestMethod]
        public async Task GenericDeleteListCommandHandler_WorkflowInputCodeType_WorkflowInputCodeTypeDto_ResultIsNotNull()
        {
            // 1. Arrange:
            var uniqueId = DateTime.Now.Ticks;

            var workflowTypeDto = new WorkflowTypeDto(uniqueId);
            var workflowDto = new WorkflowDto(uniqueId) { TypeCode = workflowTypeDto.Code };
            var workflowInputCodeTypeDto = new WorkflowInputCodeTypeDto(uniqueId) { WorkflowCode = workflowDto.Code };
            var workflowInputCodeTypeCode = workflowInputCodeTypeDto.Code;
            Assert.IsNotNull(workflowInputCodeTypeCode);

            var workflowType_entityServiceExtraTypes = new List<Type>();
            var workflow_entityServiceExtraTypes = new List<Type> { typeof(WorkflowType) };
            var workflowInputCodeType_entityServiceExtraTypes = new List<Type> { typeof(Workflow) };
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
            await GenericCreateCommandHandler_InMemory_WithSelfTest<ApplicationDbContext, WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
                workflowInputCodeTypeDto,
                workflowInputCodeType_entityServiceExtraTypes,
                applicationDbContext);

            var codes = new Collection<string> { workflowInputCodeTypeCode };

            // 2. Act:
            var response = await GenericDeleteListCommandHandlerExecute<WorkflowInputCodeType, WorkflowInputCodeTypeDto>(
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