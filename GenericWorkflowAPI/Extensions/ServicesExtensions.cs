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
        public static IServiceCollection AddDatabaseGenericCodeRepositories<TDbContext>(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
            where TDbContext : DbContext
        {
            // Mapping Example:
            //services.AddScoped(typeof(IGenericCodeRepository<Workflow>), typeof(GenericCodeRepository<Workflow, ApplicationDbContext>));

            return services.AddServices<ICodeEntity, ICodeDto>(mappings, logger,

                // IGenericCodeRepository<Workflow>
                (map) => typeof(IGenericCodeRepository<>).MakeGenericType(map.Key),

                // GenericCodeRepository<Workflow, ApplicationDbContext>
                (map) => typeof(GenericCodeRepository<,>).MakeGenericType(map.Key, typeof(TDbContext)),
                
                nameof(AddDatabaseGenericCodeRepositories));
        }

        public static IServiceCollection AddDatabaseGenericRepositories<TDbContext>(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
            where TDbContext : DbContext
        {
            // Mapping Example:
            //services.AddScoped(typeof(IGenericRepository<Workflow>), typeof(GenericRepository<Workflow, ApplicationDbContext>));

            return services.AddServices<IBaseEntity, IBaseDto>(mappings, logger,

                // IGenericRepository<Workflow>
                (map) => typeof(IGenericRepository<>).MakeGenericType(map.Key),

                // GenericRepository<Workflow, ApplicationDbContext>
                (map) => typeof(GenericRepository<,>).MakeGenericType(map.Key, typeof(TDbContext)),
                
                nameof(AddDatabaseGenericRepositories));
        }

        public static IServiceCollection AddEntityService(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
        {
            // Mapping Example:
            //services.AddScoped(typeof(IEntityService<Workflow>), typeof(EntityService<Workflow>));

            return services.AddServices<IBaseEntity, IBaseDto>(mappings, logger,

                // IEntityService<Workflow>
                (mapping) => typeof(IEntityService<>).MakeGenericType(mapping.Key),

                // EntityService<Workflow>
                (mapping) => typeof(EntityService<>).MakeGenericType(mapping.Key),
                
                nameof(AddEntityService));
        }

        public static IServiceCollection AddMediatorMappingsToServices(this IServiceCollection services, List<InterfaceImplementationMapper> mappings, ILogger logger)
        {
            if (mappings == null || mappings.Count == 0)
            {
                logger.Error("MediatR mappings null or empty");
                return services;
            }

            foreach (var mapping in mappings)
            {
                services.AddScoped(mapping.Interface, mapping.Implementation);
            }

            return services;
        }
    }
}