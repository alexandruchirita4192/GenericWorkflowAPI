using System;
using System.Collections.Generic;
using System.Text;
using GenericWorkflowAPI.Core.AutoMapper;
using GenericWorkflowAPI.Core.AutoMapper.Helpers;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GenericWorkflowAPI.Core.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void RegisterEncodingProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static void AddReflectionMappingInfoProvider(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
        {
            // Mapping Example:
            //services.AddScoped(typeof(IReflectionMappingInfoProvider<Workflow, WorkflowDto>), typeof(ReflectionMappingInfoProvider<Workflow, WorkflowDto>));

            services.AddServices<IBaseEntity, IBaseDto>(mappings, logger,

                // IReflectionMappingInfoProvider<Workflow, WorkflowDto>
                (mapping) => typeof(IReflectionMappingInfoProvider<,>).MakeGenericType(mapping.Key, mapping.Value),

                // ReflectionMappingInfoProvider<Workflow, WorkflowDto>
                (mapping) => typeof(ReflectionMappingInfoProvider<,>).MakeGenericType(mapping.Key, mapping.Value));
        }

        /// <summary>
        /// Helper used to add services using Entities and Dtos to the DI container
        /// </summary>
        /// <typeparam name="TEntityRequirement">Entity requirement (for example an interface)</typeparam>
        /// <typeparam name="TDtoRequirement">Dto requirement (for example an interface)</typeparam>
        /// <param name="services">The services collection with DI</param>
        /// <param name="mappings">The Entity-Dto mappings</param>
        /// <param name="logger">The Serilog Logger</param>
        /// <param name="getInterfaceType">The function used to generate an interface type based on received Entity type and Dto type</param>
        /// <param name="getImplementedType">The function used to generate an implemented type based on received Entity type and Dto type</param>
        public static void AddServices<TEntityRequirement, TDtoRequirement>(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger,
            Func<KeyValuePair<Type, Type>, Type> getInterfaceType,
            Func<KeyValuePair<Type, Type>, Type> getImplementedType)
        {
            if (getInterfaceType == null || getImplementedType == null)
            {
                logger.Error("One of the functions received in {procedureName} is null. Returning without adding any services.",
                    nameof(AddServices));
                return;
            }

            if (mappings == null || mappings.Count == 0)
            {
                logger.Error("No mappings received in {procedureName}. Returning without adding any services.",
                    nameof(AddServices));
                return;
            }

            foreach (var map in mappings)
            {
                Type interfaceType = typeof(string);
                Type implementedType = typeof(string);

                try
                {
                    // Check entity type
                    if (!map.Key.IsAssignableTo(typeof(TEntityRequirement)))
                    {
                        logger.Warning("Entity {entityTypeName} doesn't implement {entityRequirementTypeName}. Skipping adding this GenericRepository to the DI container.",
                            map.Key,
                            typeof(TEntityRequirement).Name);
                        continue;
                    }

                    // Check dto type
                    if (!map.Value.IsAssignableTo(typeof(TDtoRequirement)))
                    {
                        logger.Warning("{dtoTypeName} doesn't implement {dtoRequirementTypeName}. Skipping adding the mapping helper for this entity-dto pair.",
                            map.Value.Name,
                            typeof(TDtoRequirement).Name
                            );
                        continue;
                    }

                    interfaceType = getInterfaceType(map);
                    implementedType = getImplementedType(map);

                    if (!implementedType.IsAssignableTo(interfaceType))
                    {
                        logger.Warning("{implementedTypeName} doesn't implement {interfaceTypeName}. Skipping adding this GenericRepository to the DI container.",
                            implementedType.Name,
                            interfaceType.Name);
                        continue;
                    }

                    services.AddScoped(interfaceType, implementedType);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Adding GenericRepository with interface {interfaceTypeName} and implementation {implementedTypeName} failed. " +
                        "Skipping adding it to the DI container. " +
                        "Note: If some type wasn't set because of an exception, then it's type is typeof(string).Name.",
                        interfaceType.Name, implementedType.Name);
                }
            }
        }
    }
}