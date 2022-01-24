using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.CommandHandlers;
using GenericWorkflowAPI.Core.AutoMapper.Helpers;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestCommandHandlers : BaseTest
    {
        [TestMethod]
        public async Task GenericGetListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            // Request setup:
            var request = new GenericGetListRequest<WorkflowDto>
            {
                IncludePathList = new List<string> { nameof(Workflow.Type) }
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericCodeRepository<Workflow, WorkflowDto>(isInMemoryDbContext: false, logger: logger);

            // GenericGetListCommandHandler<Workflow, WorkflowDto> setup:
            var mapper = GetMapper(logger);
            var handler = new GenericGetListCommandHandler<Workflow, WorkflowDto>(repository, logger, mapper);
            var cancellationToken = new CancellationToken();

            // 2. Act:
            var response = await handler.Handle(request, cancellationToken);

            // 3. Assert:
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            Assert.IsNotNull(response.Payload);
            Assert.IsTrue((int)(response.Status ?? HttpStatusCode.OK) < 400); // 4xx-5xx are error statuses

            foreach (var item in response.Payload)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }
        }

        [TestMethod]
        public async Task GenericGetCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            // Request setup:
            var request = new GenericGetRequest<WorkflowDto>
            {
                IncludePathList = new List<string> { nameof(Workflow.Type) },
                Code = "Code637777973942605064" // TODO: This shouldn't be hard-coded
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericCodeRepository<Workflow, WorkflowDto>(isInMemoryDbContext: false, logger: logger);

            // GenericGetCommandHandler<Workflow, WorkflowDto> setup:
            var mapper = GetMapper(logger);
            var handler = new GenericGetCommandHandler<Workflow, WorkflowDto>(repository, logger, mapper);
            var cancellationToken = new CancellationToken();

            // 2. Act:
            var response = await handler.Handle(request, cancellationToken);

            // 3. Assert:
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            Assert.IsNotNull(response.Payload);
            Assert.IsTrue((int)(response.Status ?? HttpStatusCode.OK) < 400); // 4xx-5xx are error statuses

            Console.WriteLine(JsonConvert.SerializeObject(response.Payload));
        }

        [TestMethod]
        public async Task GenericCreateCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            // Request setup:
            var uniqueId = DateTime.Now.Ticks;
            var workflowDto = new WorkflowDto
            {
                Code = $"Code{uniqueId}",
                Name = $"Name{uniqueId}",
                Description = $"Description{uniqueId}",
                TypeCode = "TestCode" // TODO: This shouldn't be hard-coded
            };

            var request = new GenericCreateRequest<WorkflowDto>
            {
                Item = workflowDto,
                User = new Domain.IdentityUser("admin") { Id = 1 } // TODO: This shouldn't be hard-coded
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var configuration = GetConfiguration();
            var isInMemory = false;
            var dbContext = GetSqlServerDbContext(configuration, isInMemory);
            var entityService = GetEntityService<Workflow>();
            var repository = GetGenericCodeRepository<Workflow, WorkflowDto>(isInMemory, configuration, logger, dbContext, entityService);
            var entityServiceWorkflowType = GetEntityService<WorkflowType>();

            // GenericCreateCommandHandler<Workflow, WorkflowDto> setup:
            var mapper = GetMapper(logger);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var reflectionMappingInfoProvider = new ReflectionMappingInfoProvider<Workflow, WorkflowDto>(logger, memoryCache);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<DbContext>(dbContext);
            serviceCollection.AddSingleton(dbContext);
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection.AddSingleton<IEntityService<Workflow>>(entityService);
            serviceCollection.AddSingleton<IEntityService<WorkflowType>>(entityServiceWorkflowType);
            serviceCollection.AddSingleton<IGenericRepository<Workflow>>(repository);
            serviceCollection.AddSingleton<IGenericCodeRepository<Workflow>>(repository);
            serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddSingleton<IReflectionMappingInfoProvider<Workflow, WorkflowDto>>(reflectionMappingInfoProvider);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mappingHelper = new MappingHelper<Workflow, WorkflowDto>(logger, mapper, reflectionMappingInfoProvider, serviceProvider);
            var handler = new GenericCreateCommandHandler<Workflow, WorkflowDto>(repository, logger, mappingHelper);
            var cancellationToken = new CancellationToken();

            // 2. Act:
            var response = await handler.Handle(request, cancellationToken);

            // 3. Assert:
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            Assert.IsNotNull(response.Payload);
            Assert.AreEqual(HttpStatusCode.Created, response.Status, "HttpStatus was not Created.");

            Console.WriteLine(JsonConvert.SerializeObject(response.Payload));
        }

        [TestMethod]
        public async Task GenericCreateListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            // Request setup:
            var uniqueId = DateTime.Now.Ticks;
            var workflowDto = new WorkflowDto
            {
                Code = $"Code{uniqueId}",
                Name = $"Name{uniqueId}",
                Description = $"Description{uniqueId}",
                TypeCode = "TestCode" // TODO: This shouldn't be hard-coded
            };

            var request = new GenericCreateListRequest<WorkflowDto>
            {
                Collection = new Collection<WorkflowDto> { workflowDto },
                User = new Domain.IdentityUser("admin") { Id = 1 } // TODO: This shouldn't be hard-coded
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var configuration = GetConfiguration();
            var isInMemory = false;
            var dbContext = GetSqlServerDbContext(configuration, isInMemory);
            var entityService = GetEntityService<Workflow>();
            var repository = GetGenericCodeRepository<Workflow, WorkflowDto>(isInMemory, configuration, logger, dbContext, entityService);
            var entityServiceWorkflowType = GetEntityService<WorkflowType>();

            // GenericCreateListCommandHandler<Workflow, WorkflowDto> setup:
            var mapper = GetMapper(logger);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var reflectionMappingInfoProvider = new ReflectionMappingInfoProvider<Workflow, WorkflowDto>(logger, memoryCache);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<DbContext>(dbContext);
            serviceCollection.AddSingleton(dbContext);
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection.AddSingleton<IEntityService<Workflow>>(entityService);
            serviceCollection.AddSingleton<IEntityService<WorkflowType>>(entityServiceWorkflowType);
            serviceCollection.AddSingleton<IGenericRepository<Workflow>>(repository);
            serviceCollection.AddSingleton<IGenericCodeRepository<Workflow>>(repository);
            serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddSingleton<IReflectionMappingInfoProvider<Workflow, WorkflowDto>>(reflectionMappingInfoProvider);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mappingHelper = new MappingHelper<Workflow, WorkflowDto>(logger, mapper, reflectionMappingInfoProvider, serviceProvider);
            var handler = new GenericCreateListCommandHandler<Workflow, WorkflowDto>(repository, logger, mappingHelper);
            var cancellationToken = new CancellationToken();

            // 2. Act:
            var response = await handler.Handle(request, cancellationToken);

            // 3. Assert:
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            Assert.IsNotNull(response.Payload);
            Assert.AreEqual(HttpStatusCode.Created, response.Status, "HttpStatus was not Created.");

            Console.WriteLine(JsonConvert.SerializeObject(response.Payload));
        }

        [TestMethod]
        public async Task GenericUpdateCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            // Request setup:
            var uniqueId = DateTime.Now.Ticks;
            var workflowDto = new WorkflowDto
            {
                Code = $"Code637777973942605064",
                Name = $"Name{uniqueId}",
                Description = $"Description{uniqueId}",
                TypeCode = "TestCode" // TODO: This shouldn't be hard-coded
            };

            var request = new GenericUpdateRequest<WorkflowDto>
            {
                Item = workflowDto,
                User = new Domain.IdentityUser("admin") { Id = 1 } // TODO: This shouldn't be hard-coded
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var configuration = GetConfiguration();
            var isInMemory = false;
            var dbContext = GetSqlServerDbContext(configuration, isInMemory);
            var entityService = GetEntityService<Workflow>();
            var repository = GetGenericCodeRepository<Workflow, WorkflowDto>(isInMemory, configuration, logger, dbContext, entityService);
            var entityServiceWorkflowType = GetEntityService<WorkflowType>();

            // GenericCreateListCommandHandler<Workflow, WorkflowDto> setup:
            var mapper = GetMapper(logger);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var reflectionMappingInfoProvider = new ReflectionMappingInfoProvider<Workflow, WorkflowDto>(logger, memoryCache);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<DbContext>(dbContext);
            serviceCollection.AddSingleton(dbContext);
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection.AddSingleton<IEntityService<Workflow>>(entityService);
            serviceCollection.AddSingleton<IEntityService<WorkflowType>>(entityServiceWorkflowType);
            serviceCollection.AddSingleton<IGenericRepository<Workflow>>(repository);
            serviceCollection.AddSingleton<IGenericCodeRepository<Workflow>>(repository);
            serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddSingleton<IReflectionMappingInfoProvider<Workflow, WorkflowDto>>(reflectionMappingInfoProvider);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mappingHelper = new MappingHelper<Workflow, WorkflowDto>(logger, mapper, reflectionMappingInfoProvider, serviceProvider);
            var handler = new GenericUpdateCommandHandler<Workflow, WorkflowDto>(repository, logger, mappingHelper);
            var cancellationToken = new CancellationToken();

            // 2. Act:
            var response = await handler.Handle(request, cancellationToken);

            // 3. Assert:
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            Assert.IsNull(response.Payload);
            Assert.AreEqual(HttpStatusCode.OK, response.Status, "HttpStatus was not OK.");

            Console.WriteLine(JsonConvert.SerializeObject(response.Payload));
        }

        [TestMethod]
        public async Task GenericUpdateListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            // Request setup:
            var uniqueId = DateTime.Now.Ticks;
            var workflowDto = new WorkflowDto
            {
                Code = $"Code637777973942605064",
                Name = $"Name{uniqueId}",
                Description = $"Description{uniqueId}",
                TypeCode = "TestCode" // TODO: This shouldn't be hard-coded
            };

            var request = new GenericUpdateListRequest<WorkflowDto>
            {
                Collection = new Collection<WorkflowDto> { workflowDto },
                User = new Domain.IdentityUser("admin") { Id = 1 } // TODO: This shouldn't be hard-coded
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var configuration = GetConfiguration();
            var isInMemory = false;
            var dbContext = GetSqlServerDbContext(configuration, isInMemory);
            var entityService = GetEntityService<Workflow>();
            var repository = GetGenericCodeRepository<Workflow, WorkflowDto>(isInMemory, configuration, logger, dbContext, entityService);
            var entityServiceWorkflowType = GetEntityService<WorkflowType>();

            // GenericCreateListCommandHandler<Workflow, WorkflowDto> setup:
            var mapper = GetMapper(logger);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var reflectionMappingInfoProvider = new ReflectionMappingInfoProvider<Workflow, WorkflowDto>(logger, memoryCache);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<DbContext>(dbContext);
            serviceCollection.AddSingleton(dbContext);
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection.AddSingleton<IEntityService<Workflow>>(entityService);
            serviceCollection.AddSingleton<IEntityService<WorkflowType>>(entityServiceWorkflowType);
            serviceCollection.AddSingleton<IGenericRepository<Workflow>>(repository);
            serviceCollection.AddSingleton<IGenericCodeRepository<Workflow>>(repository);
            serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddSingleton<IReflectionMappingInfoProvider<Workflow, WorkflowDto>>(reflectionMappingInfoProvider);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mappingHelper = new MappingHelper<Workflow, WorkflowDto>(logger, mapper, reflectionMappingInfoProvider, serviceProvider);
            var handler = new GenericUpdateListCommandHandler<Workflow, WorkflowDto>(repository, logger, mappingHelper);
            var cancellationToken = new CancellationToken();

            // 2. Act:
            var response = await handler.Handle(request, cancellationToken);

            // 3. Assert:
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            Assert.IsNull(response.Payload);
            Assert.AreEqual(HttpStatusCode.OK, response.Status, "HttpStatus was not OK.");

            Console.WriteLine(JsonConvert.SerializeObject(response.Payload));
        }

        [TestMethod]
        public async Task GenericDeleteCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            // Request setup:
            var request = new GenericDeleteRequest<WorkflowDto>
            {
                Code = "Code637777971885190442", // TODO: This shouldn't be hard-coded
                User = new Domain.IdentityUser("admin") { Id = 1 } // minimum user requirements
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericCodeRepository<Workflow, WorkflowDto>(logger: logger);

            // GenericDeleteCommandHandler<Workflow, WorkflowDto> setup:
            var handler = new GenericDeleteCommandHandler<Workflow, WorkflowDto>(repository, logger);
            var cancellationToken = new CancellationToken();

            // 2. Act:
            var response = await handler.Handle(request, cancellationToken);

            // 3. Assert:
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            Assert.IsNull(response.Payload);
            Assert.AreEqual(HttpStatusCode.OK, response.Status, "HttpStatus was not OK.");

            Console.WriteLine(JsonConvert.SerializeObject(response.Payload));
        }

        [TestMethod]
        public async Task GenericDeleteListCommandHandler_Workflow_WorkflowDto_ResultIsNotNull()
        {
            // 1. Arrange:

            // Request setup:
            var request = new GenericDeleteListRequest<WorkflowDto>
            {
                Codes = new Collection<string> { "Code637777967760777833" }, // TODO: This shouldn't be hard-coded
                User = new Domain.IdentityUser("admin") { Id = 1 } // minimum user requirements
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericCodeRepository<Workflow, WorkflowDto>(logger: logger);

            // GenericDeleteCommandHandler<Workflow, WorkflowDto> setup:
            var handler = new GenericDeleteListCommandHandler<Workflow, WorkflowDto>(repository, logger);
            var cancellationToken = new CancellationToken();

            // 2. Act:
            var response = await handler.Handle(request, cancellationToken);

            // 3. Assert:
            Assert.IsNotNull(response);
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Message));
            Assert.IsNull(response.Payload);
            Assert.AreEqual(HttpStatusCode.OK, response.Status, "HttpStatus was not OK.");

            Console.WriteLine(JsonConvert.SerializeObject(response.Payload));
        }
    }
}