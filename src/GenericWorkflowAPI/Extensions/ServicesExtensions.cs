using System;
using System.Collections.Generic;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.Core.Extensions;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GenericWorkflowAPI.Extensions
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add code repository services as singleton with interface <see cref="IGenericCodeRepository{TEntity}"/> implementing
        ///  <see cref="GenericCodeRepository{TEntity, TDbContext}"/> to the <paramref name="services"/> container based on <paramref name="mappings"/> dictionary.
        /// </summary>
        public static IServiceCollection AddDatabaseGenericCodeRepositories<TDbContext>(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
            where TDbContext : DbContext
        {
            // Mapping Example:
            //services.AddSingleton(typeof(IGenericCodeRepository<Workflow>), typeof(GenericCodeRepository<Workflow, ApplicationDbContext>));

            return services.AddServices<ICodeEntity, ICodeDto>(mappings, logger,

                // IGenericCodeRepository<Workflow>
                (map) => typeof(IGenericCodeRepository<>).MakeGenericType(map.Key),

                // GenericCodeRepository<Workflow, ApplicationDbContext>
                (map) => typeof(GenericCodeRepository<,>).MakeGenericType(map.Key, typeof(TDbContext)),

                nameof(AddDatabaseGenericCodeRepositories),
                ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Add repository services as singleton with interface <see cref="IGenericRepository{TEntity}"/> implementing
        ///  <see cref="GenericRepository{TEntity, TDbContext}"/> to the <paramref name="services"/> container based on <paramref name="mappings"/> dictionary.
        /// </summary>
        public static IServiceCollection AddDatabaseGenericRepositories<TDbContext>(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
            where TDbContext : DbContext
        {
            // Mapping Example:
            //services.AddSingleton(typeof(IGenericRepository<Workflow>), typeof(GenericRepository<Workflow, ApplicationDbContext>));

            return services.AddServices<IBaseEntity, IBaseDto>(mappings, logger,

                // IGenericRepository<Workflow>
                (map) => typeof(IGenericRepository<>).MakeGenericType(map.Key),

                // GenericRepository<Workflow, ApplicationDbContext>
                (map) => typeof(GenericRepository<,>).MakeGenericType(map.Key, typeof(TDbContext)),

                nameof(AddDatabaseGenericRepositories),
                ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Add entity-related services as singletons (<see cref="IEntityService{TEntity}"/> implemented as <see cref="EntityService{TEntity}"/>).
        /// </summary>
        public static IServiceCollection AddEntityService(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
        {
            // Mapping Example:
            //services.AddScoped(typeof(IEntityService<Workflow>), typeof(EntityService<Workflow>));

            return services.AddServices<IBaseEntity, IBaseDto>(mappings, logger,

                // IEntityService<Workflow>
                (mapping) => typeof(IEntityService<>).MakeGenericType(mapping.Key),

                // EntityService<Workflow>
                (mapping) => typeof(EntityService<>).MakeGenericType(mapping.Key),

                nameof(AddEntityService),
                ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Add mapping-related services as singletons (<see cref="IMappingHelper{TEntity, TDto}"/> implemented as <see cref="MappingHelper{TEntity, TDto}"/>).
        /// </summary>
        public static IServiceCollection AddMappingHelpers<TDbContext>(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
            where TDbContext : DbContext
        {
            // Mapping Example:
            //services.AddSingleton(typeof(IMappingHelper<ApplicationDbContext, Workflow, WorkflowDto>), typeof(MappingHelper<ApplicationDbContext, Workflow, WorkflowDto>));

            var dbContextType = typeof(TDbContext);

            return services.AddServices<IBaseEntity, IBaseDto>(mappings, logger,

                // IMappingHelper<ApplicationDbContext, Workflow, WorkflowDto>
                (mapping) => typeof(IMappingHelper<,,>).MakeGenericType(dbContextType, mapping.Key, mapping.Value),

                // MappingHelper<ApplicationDbContext, Workflow, WorkflowDto>
                (mapping) => typeof(MappingHelper<,,>).MakeGenericType(dbContextType, mapping.Key, mapping.Value),

                nameof(AddMappingHelpers),
                ServiceLifetime.Singleton);
        }


        /// <summary>
        /// Add MediatR handlers from <paramref name="servicePairs"/> to <paramref name="services"/> as singletons.
        /// </summary>
        public static IServiceCollection AddMediatRHandlersToServices(this IServiceCollection services, List<ServiceInterfaceImplementationPair> servicePairs, ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (servicePairs == null || servicePairs.Count == 0)
            {
                logger.Error("MediatR service interface-implementation pairs null or empty");
                return services;
            }

            foreach (var service in servicePairs)
            {
                services.AddService(service.Interface, service.Implementation, ServiceLifetime.Singleton, logger);
            }

            return services;
        }
    }
}