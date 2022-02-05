using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.CommandHandlers;
using GenericWorkflowAPI.CommandHandlers.RequestHandlers;
using GenericWorkflowAPI.Core.AutoMapper.Helpers;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using GenericWorkflowAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace GenericWorkflowAPI.UnitTesting
{
    public class GenericCommandHandlerTest : BaseTest
    {
        public async Task<GenericApiResponse<List<TDto>>> GenericGetListCommandHandlerExecute<TEntity, TDto>(
            List<string> includePathList,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
            where TEntity : class, IBaseEntity, new()
            where TDto : class, IBaseDto, new()
        {
            // Request setup:
            var request = new GenericGetListRequest<TDto>
            {
                IncludePathList = includePathList
            };

            // GenericCodeRepository<TEntity, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericRepository<TEntity>(isInMemoryDbContext, dbContext: dbContext, logger: logger);

            // GenericGetListCommandHandler<TEntity, TDto> setup:
            var mapper = GetMapper(logger);
            var handler = new GenericGetListCommandHandler<TEntity, TDto>(repository, logger, mapper);
            var cancellationToken = new CancellationToken();

            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        public async Task<GenericApiResponse<TDto>> GenericGetCommandHandlerExecute<TEntity, TDto>(
            List<string> includePathList,
            string code,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, ICodeDto, new()
        {
            // Request setup:
            var request = new GenericGetRequest<TDto>
            {
                IncludePathList = includePathList,
                Code = code
            };

            // GenericCodeRepository<TEntity, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext, dbContext: dbContext, logger: logger);

            // GenericGetCommandHandler<TEntity, TDto> setup:
            var mapper = GetMapper(logger);
            var handler = new GenericGetCommandHandler<TEntity, TDto>(repository, logger, mapper);
            var cancellationToken = new CancellationToken();

            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        public async Task<GenericApiResponse<string>> GenericCreateCommandHandlerExecute<TEntity, TDto>(
            TDto dto,
            Domain.IdentityUser user,
            List<Type> entityServiceExtraTypes,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, ICodeDto, new()
        {
            // Request setup:
            var request = new GenericCreateRequest<TDto>
            {
                Item = dto,
                User = user
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var configuration = GetConfiguration();
            if (dbContext == null)
                dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            var entityService = GetEntityService<TEntity>();
            var repository = GetGenericCodeRepository(isInMemoryDbContext, configuration, logger, dbContext, entityService);

            // GenericCreateCommandHandler<Workflow, WorkflowDto> setup:
            var mapper = GetMapper(logger);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var reflectionMappingInfoProvider = new ReflectionMappingInfoProvider<TEntity, TDto>(logger, memoryCache);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton(dbContext);
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection.AddSingleton<IEntityService<TEntity>>(entityService);

            FillEntityServiceForExtraTypes(serviceCollection, entityServiceExtraTypes);

            serviceCollection.AddSingleton<IGenericRepository<TEntity>>(repository);
            serviceCollection.AddSingleton<IGenericCodeRepository<TEntity>>(repository);
            serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddSingleton<IReflectionMappingInfoProvider<TEntity, TDto>>(reflectionMappingInfoProvider);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mappingHelper = new MappingHelper<TEntity, TDto>(logger, mapper, reflectionMappingInfoProvider, serviceProvider);
            var handler = new GenericCreateCommandHandler<TEntity, TDto>(repository, logger, mappingHelper);
            var cancellationToken = new CancellationToken();

            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        public async Task<GenericApiResponse<string>> GenericCreateListCommandHandlerExecute<TEntity, TDto>(
            Collection<TDto> dtoCollection,
            Domain.IdentityUser user,
            List<Type> entityServiceExtraTypes,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, ICodeDto, new()
        {
            // Request setup:
            var request = new GenericCreateListRequest<TDto>
            {
                Collection = dtoCollection,
                User = user
            };

            // GenericCodeRepository<TEntity, ApplicationDbContext> setup:
            var logger = GetLogger();
            var configuration = GetConfiguration();
            if (dbContext == null)
                dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            var entityService = GetEntityService<TEntity>();
            var repository = GetGenericCodeRepository(isInMemoryDbContext, configuration, logger, dbContext, entityService);

            // GenericCreateListCommandHandler<TEntity, TDto> setup:
            var mapper = GetMapper(logger);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var reflectionMappingInfoProvider = new ReflectionMappingInfoProvider<TEntity, TDto>(logger, memoryCache);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton(dbContext);
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection.AddSingleton<IEntityService<TEntity>>(entityService);

            FillEntityServiceForExtraTypes(serviceCollection, entityServiceExtraTypes);

            serviceCollection.AddSingleton<IGenericRepository<TEntity>>(repository);
            serviceCollection.AddSingleton<IGenericCodeRepository<TEntity>>(repository);
            serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddSingleton<IReflectionMappingInfoProvider<TEntity, TDto>>(reflectionMappingInfoProvider);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mappingHelper = new MappingHelper<TEntity, TDto>(logger, mapper, reflectionMappingInfoProvider, serviceProvider);
            var handler = new GenericCreateListCommandHandler<TEntity, TDto>(repository, logger, mappingHelper);

            var cancellationToken = new CancellationToken();
            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        public async Task<GenericApiResponse<string>> GenericUpdateCommandHandlerExecute<TEntity, TDto>(
            TDto dto,
            Domain.IdentityUser user,
            List<Type> entityServiceExtraTypes,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, ICodeDto, new()
        {
            // Request setup:
            var request = new GenericUpdateRequest<TDto>
            {
                Item = dto,
                User = user
            };

            // GenericCodeRepository<TEntity, ApplicationDbContext> setup:
            var logger = GetLogger();
            var configuration = GetConfiguration();
            if (dbContext == null)
                dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            var entityService = GetEntityService<TEntity>();
            var repository = GetGenericCodeRepository(isInMemoryDbContext, configuration, logger, dbContext, entityService);

            // GenericCreateListCommandHandler<TEntity, TDto> setup:
            var mapper = GetMapper(logger);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var reflectionMappingInfoProvider = new ReflectionMappingInfoProvider<TEntity, TDto>(logger, memoryCache);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton(dbContext);
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection.AddSingleton<IEntityService<TEntity>>(entityService);

            FillEntityServiceForExtraTypes(serviceCollection, entityServiceExtraTypes);

            serviceCollection.AddSingleton<IGenericRepository<TEntity>>(repository);
            serviceCollection.AddSingleton<IGenericCodeRepository<TEntity>>(repository);
            serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddSingleton<IReflectionMappingInfoProvider<TEntity, TDto>>(reflectionMappingInfoProvider);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mappingHelper = new MappingHelper<TEntity, TDto>(logger, mapper, reflectionMappingInfoProvider, serviceProvider);
            var handler = new GenericUpdateCommandHandler<TEntity, TDto>(repository, logger, mappingHelper);
            var cancellationToken = new CancellationToken();

            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        public async Task<GenericApiResponse<string>> GenericUpdateListCommandHandlerExecute<TEntity, TDto>(
            TDto dto,
            Domain.IdentityUser user,
            List<Type> entityServiceExtraTypes,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, ICodeDto, new()
        {
            // Request setup:
            var request = new GenericUpdateListRequest<TDto>
            {
                Collection = new Collection<TDto> { dto },
                User = user
            };

            // GenericCodeRepository<TEntity, ApplicationDbContext> setup:
            var logger = GetLogger();
            var configuration = GetConfiguration();
            if (dbContext == null)
                dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            var entityService = GetEntityService<TEntity>();
            var repository = GetGenericCodeRepository(isInMemoryDbContext, configuration, logger, dbContext, entityService);

            // GenericCreateListCommandHandler<TEntity, TDto> setup:
            var mapper = GetMapper(logger);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var reflectionMappingInfoProvider = new ReflectionMappingInfoProvider<TEntity, TDto>(logger, memoryCache);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton(dbContext);
            serviceCollection.AddSingleton<ILogger>(logger);
            serviceCollection.AddSingleton<IEntityService<TEntity>>(entityService);

            FillEntityServiceForExtraTypes(serviceCollection, entityServiceExtraTypes);

            serviceCollection.AddSingleton<IGenericRepository<TEntity>>(repository);
            serviceCollection.AddSingleton<IGenericCodeRepository<TEntity>>(repository);
            serviceCollection.AddSingleton<IMemoryCache>(memoryCache);
            serviceCollection.AddSingleton<IReflectionMappingInfoProvider<TEntity, TDto>>(reflectionMappingInfoProvider);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mappingHelper = new MappingHelper<TEntity, TDto>(logger, mapper, reflectionMappingInfoProvider, serviceProvider);
            var handler = new GenericUpdateListCommandHandler<TEntity, TDto>(repository, logger, mappingHelper);
            var cancellationToken = new CancellationToken();

            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        public async Task<GenericApiResponse<string>> GenericDeleteCommandHandlerExecute<TEntity, TDto>(
            string code,
            Domain.IdentityUser user,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, ICodeDto, new()
        {
            // Request setup:
            var request = new GenericDeleteRequest<TDto>
            {
                Code = code,
                User = user
            };

            // GenericCodeRepository<TEntity, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext: isInMemoryDbContext, dbContext: dbContext, logger: logger);

            // GenericDeleteCommandHandler<TEntity, TDto> setup:
            var handler = new GenericDeleteCommandHandler<TEntity, TDto>(repository, logger);
            var cancellationToken = new CancellationToken();

            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        public async Task<GenericApiResponse<string>> GenericDeleteListCommandHandlerExecute<TEntity, TDto>(
            Collection<string> codes,
            Domain.IdentityUser user,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, ICodeDto, new()
        {
            // Request setup:
            var request = new GenericDeleteListRequest<TDto>
            {
                Codes = codes,
                User = user
            };

            // GenericCodeRepository<Workflow, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext: isInMemoryDbContext, dbContext: dbContext, logger: logger);

            // GenericDeleteListCommandHandler<Workflow, WorkflowDto> setup:
            var handler = new GenericDeleteListCommandHandler<TEntity, TDto>(repository, logger);
            var cancellationToken = new CancellationToken();
            var response = await handler.Handle(request, cancellationToken);

            return response;
        }

        public async Task GenericCreateCommandHandler_InMemory_WithSelfTest<TEntity, TDto>(
            TDto dto,
            List<Type> entityServiceExtraTypes,
            ApplicationDbContext dbContext)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, ICodeDto, new()
        {
            // 1. Arrange:
            var user = GetDefaultUser();

            // 2. Act:
            var response = await GenericCreateCommandHandlerExecute<TEntity, TDto>(
                dto,
                user,
                entityServiceExtraTypes,
               true,
                dbContext);

            // 3. Assert:
            AssertGenericApiResponse(response, HttpStatusCode.Created);
        }

        public async Task<GenericApiResponse<string>> GenericExecuteWorkflowCommandHandlerExecute(
            string workflowCode,
            string workflowInstanceCode,
            Dictionary<string, string> workflowInputCodeTypeXvalue,
            List<string> roles,
            bool isInMemoryDbContext,
            ApplicationDbContext? dbContext = null)
        {
            if (dbContext == null)
                dbContext = GetSqlServerDbContext(null, isInMemoryDbContext);
            // The user is the default one:
            var user = GetDefaultUser();

            // Request setup:
            var request = new ExecuteWorkflowRequest
            {
                User = user,
                WorkflowCode = workflowCode,
                WorkflowInstanceCode = workflowInstanceCode,
                WorkflowInputCodeTypeXvalue = workflowInputCodeTypeXvalue
            };

            var logger = GetLogger();
            var workflowRepository = GetGenericCodeRepository<Workflow>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var workflowInputCodeTypeRepository = GetGenericCodeRepository<WorkflowInputCodeType>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var workflowStateRepository = GetGenericCodeRepository<WorkflowState>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var workflowStateInputCodeTypeRepository = GetGenericCodeRepository<WorkflowStateInputCodeType>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var workflowTransitionRepository = GetGenericCodeRepository<WorkflowTransition>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var workflowInstanceRepository = GetGenericCodeRepository<WorkflowInstance>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var workflowInstanceInputCodeRepository = GetGenericRepository<WorkflowInstanceInputCode>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var workflowInstanceHistoryRepository = GetGenericRepository<WorkflowInstanceHistory>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var workflowInstanceHistoryInputCodeRepository = GetGenericRepository<WorkflowInstanceHistoryInputCode>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var identityRoleRepository = GetGenericCodeRepository<Domain.IdentityRole>(isInMemoryDbContext, dbContext: dbContext, logger: logger);
            var userManager = GetBasicUserManager(dbContext, isInMemoryDbContext);

            if (roles.Count != 0)
            {
                // Assert user
                var loadedUser = await dbContext.Users.FindAsync(user.Id);
                Assert.IsNotNull(loadedUser);

                // Assert roles
                foreach(var role in roles)
                {
                    var loadedRole = await dbContext.Roles.FirstOrDefaultAsync(r=>r.Code == role);
                    Print(loadedRole, $"Role loaded with Code={role}");
                    Assert.IsNotNull(loadedRole);
                    loadedRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == role);
                    Print(loadedRole, $"Role loaded with Name={role}");
                    Assert.IsNotNull(loadedRole);
                    loadedRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.NormalizedName == role);
                    Print(loadedRole, $"Role loaded with NormalizedName={role}");
                    Assert.IsNotNull(loadedRole);
                }

                // Add required roles from list to user
                var result = await userManager.AddToRolesAsync(loadedUser, roles);
                Print(result, "UserManager AddToRolesAsync result:");
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Succeeded);
            }

            var workflowService = new WorkflowService(
                logger,
                workflowRepository,
                workflowInputCodeTypeRepository,
                workflowStateRepository,
                workflowStateInputCodeTypeRepository,
                workflowTransitionRepository,
                workflowInstanceRepository,
                workflowInstanceInputCodeRepository,
                workflowInstanceHistoryRepository,
                workflowInstanceHistoryInputCodeRepository,
                identityRoleRepository,
                userManager
                );
            var handler = new ExecuteWorkflowCommandHandler(workflowService, logger);
            var cancellationToken = new CancellationToken();

            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        protected UserManager<Domain.IdentityUser> GetBasicUserManager(ApplicationDbContext? dbContext = null, bool? isInMemoryDbContext = null)
        {
            if (dbContext == null)
                dbContext = GetSqlServerDbContext(null, isInMemoryDbContext);

            var store = new UserStore<Domain.IdentityUser, Domain.IdentityRole, ApplicationDbContext, long>(dbContext);
            var microsoftLogger = new Microsoft.Extensions.Logging.Logger<UserManager<Domain.IdentityUser>>(new Microsoft.Extensions.Logging.LoggerFactory());
            var userManager = new UserManager<Domain.IdentityUser>(
                store,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                microsoftLogger);
            return userManager;
        }

        private void FillEntityServiceForExtraTypes(ServiceCollection serviceCollection, List<Type> entityServiceExtraTypes)
        {
            // Example of what this does for a type WorkflowType:
            //var entityServiceWorkflowType = GetEntityService<WorkflowType>();
            //serviceCollection.AddSingleton<IEntityService<WorkflowType>>(entityServiceWorkflowType);

            if (entityServiceExtraTypes == null || entityServiceExtraTypes.Count == 0 || serviceCollection == null)
                return;

            foreach (var type in entityServiceExtraTypes)
            {
                var entityServiceMethod = typeof(BaseTest).GetMethod(nameof(BaseTest.GetEntityService));
                if (entityServiceMethod == null)
                    throw new InvalidOperationException($"Missing method {nameof(BaseTest.GetEntityService)} in type {typeof(BaseTest).Name}");

                var genericEntityServiceMethod = entityServiceMethod.MakeGenericMethod(type);
                var entityServiceInstance = genericEntityServiceMethod.Invoke(this, null); // call GetEntityService<TEntity>()
                if (entityServiceInstance == null)
                    throw new InvalidOperationException($"Returned null instance of type {type.Name} after calling {nameof(BaseTest.GetEntityService)}");

                var entityServiceInstanceInterfaceType = typeof(IEntityService<>).MakeGenericType(type);

                serviceCollection.AddSingleton(entityServiceInstanceInterfaceType, entityServiceInstance);
            }
        }
    }
}