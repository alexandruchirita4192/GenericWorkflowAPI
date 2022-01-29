using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.CommandHandlers;
using GenericWorkflowAPI.Core.AutoMapper.Helpers;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GenericWorkflowAPI.UnitTesting
{
    public class GenericCommandHandlerTest : BaseTest
    {
        public async Task<GenericApiResponse<List<TDto>>> GenericGetListCommandHandlerExecute<TEntity, TDto>(
            List<string> includePathList,
            bool isInMemoryDbContext)
            where TEntity : class, IBaseEntity, ICodeEntity, new()
            where TDto : class, IBaseDto, new()
        {
            // Request setup:
            var request = new GenericGetListRequest<TDto>
            {
                IncludePathList = includePathList
            };

            // GenericCodeRepository<TEntity, ApplicationDbContext> setup:
            var logger = GetLogger();
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext, logger: logger);

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
            bool isInMemoryDbContext)
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
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext, logger: logger);

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
            bool isInMemoryDbContext)
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
            var dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            var entityService = GetEntityService<TEntity>();
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext, configuration, logger, dbContext, entityService);

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
            bool isInMemoryDbContext)
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
            var dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            var entityService = GetEntityService<TEntity>();
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext, configuration, logger, dbContext, entityService);

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
            bool isInMemoryDbContext)
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
            var dbContext = GetSqlServerDbContext(configuration, isInMemoryDbContext);
            var entityService = GetEntityService<TEntity>();
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext, configuration, logger, dbContext, entityService);

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
            bool isInMemoryDbContext)
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
            var isInMemory = false;
            var dbContext = GetSqlServerDbContext(configuration, isInMemory);
            var entityService = GetEntityService<TEntity>();
            var repository = GetGenericCodeRepository<TEntity>(isInMemory, configuration, logger, dbContext, entityService);

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
            bool isInMemoryDbContext)
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
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext: isInMemoryDbContext, logger: logger);

            // GenericDeleteCommandHandler<TEntity, TDto> setup:
            var handler = new GenericDeleteCommandHandler<TEntity, TDto>(repository, logger);
            var cancellationToken = new CancellationToken();

            var response = await handler.Handle(request, cancellationToken);
            return response;
        }

        public async Task<GenericApiResponse<string>> GenericDeleteListCommandHandlerExecute<TEntity, TDto> (
            Collection<string> codes,
            Domain.IdentityUser user,
            bool isInMemoryDbContext)
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
            var repository = GetGenericCodeRepository<TEntity>(isInMemoryDbContext: isInMemoryDbContext, logger: logger);

            // GenericDeleteListCommandHandler<Workflow, WorkflowDto> setup:
            var handler = new GenericDeleteListCommandHandler<TEntity, TDto>(repository, logger);
            var cancellationToken = new CancellationToken();
            var response = await handler.Handle(request, cancellationToken);

            return response;
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