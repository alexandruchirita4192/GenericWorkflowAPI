using System;
using System.Collections.Generic;
using System.Text;
using GenericWorkflowAPI.Core.AutoMapper.Helpers;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using GenericWorkflowAPI.Core.AutoMapper;

namespace GenericWorkflowAPI.Core.Extensions
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Configure Cors Policy allowing any origin, method and header.
        /// </summary>
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        /// <summary>
        /// Register supported encodings (might be required for some console logging using an encoding or configuration reading from file having an encoding).
        /// </summary>
        public static void RegisterEncodingProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Add singleton services that provide <see cref="ReflectionMappingInfo"/> list with <see cref="IReflectionMappingInfoProvider{TEntity, TDto}"/> implemented as
        ///  <see cref="ReflectionMappingInfoProvider{TEntity, TDto}"/> to the <paramref name="services"/> container based on <paramref name="mappings"/> dictionary.
        /// </summary>
        public static IServiceCollection AddReflectionMappingInfoProvider(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger)
        {
            // Mapping Example:
            //services.AddSingleton(typeof(IReflectionMappingInfoProvider<Workflow, WorkflowDto>), typeof(ReflectionMappingInfoProvider<Workflow, WorkflowDto>));

            return services.AddServices<IBaseEntity, IBaseDto>(mappings, logger,

                // IReflectionMappingInfoProvider<Workflow, WorkflowDto>
                (mapping) => typeof(IReflectionMappingInfoProvider<,>).MakeGenericType(mapping.Key, mapping.Value),

                // ReflectionMappingInfoProvider<Workflow, WorkflowDto>
                (mapping) => typeof(ReflectionMappingInfoProvider<,>).MakeGenericType(mapping.Key, mapping.Value),

                nameof(AddReflectionMappingInfoProvider),
                ServiceLifetime.Singleton);
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
        public static IServiceCollection AddServices<TEntityRequirement, TDtoRequirement>(this IServiceCollection services, Dictionary<Type, Type> mappings, ILogger logger,
            Func<KeyValuePair<Type, Type>, Type> getInterfaceType,
            Func<KeyValuePair<Type, Type>, Type> getImplementedType,
            string procedureName,
            ServiceLifetime serviceLifetime)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (getInterfaceType == null || getImplementedType == null)
            {
                logger.Error("One of the functions received in {procedureName} is null. Returning without adding any services.",
                    procedureName ?? nameof(AddServices));
                return services;
            }

            if (mappings == null || mappings.Count == 0)
            {
                logger.Error("No mappings received in {procedureName}. Returning without adding any services.",
                    procedureName ?? nameof(AddServices));
                return services;
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
                        logger.Warning("Entity {entityTypeName} doesn't implement {entityRequirementTypeName}. Skipping adding this service to the DI container in {procedureName}.",
                            map.Key,
                            typeof(TEntityRequirement).Name,
                            procedureName ?? nameof(AddServices)
                            );
                        continue;
                    }

                    // Check dto type
                    if (!map.Value.IsAssignableTo(typeof(TDtoRequirement)))
                    {
                        logger.Warning("{dtoTypeName} doesn't implement {dtoRequirementTypeName}. Skipping adding this service to the DI container in {procedureName}.",
                            map.Value.Name,
                            typeof(TDtoRequirement).Name,
                            procedureName ?? nameof(AddServices)
                            );
                        continue;
                    }

                    interfaceType = getInterfaceType(map);
                    implementedType = getImplementedType(map);

                    services.AddService(interfaceType, implementedType, serviceLifetime, logger);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Adding GenericRepository with interface {interfaceTypeName} and implementation {implementedTypeName} failed. " +
                        "Skipping adding it to the DI container in {procedureName}. " +
                        "Note: If some type wasn't set because of an exception, then it's type is typeof(string).Name.",
                        interfaceType.Name,
                        implementedType.Name,
                        procedureName ?? nameof(AddServices)
                        );
                }
            }

            return services;
        }

        /// <summary>
        /// Add <paramref name="implementedType"/> service as an interface <paramref name="interfaceType"/> to the <paramref name="services"/>
        ///  dependency injection container with the <see cref="ServiceLifetime"/> lifetime.
        /// </summary>
        public static void AddService(this IServiceCollection services, Type interfaceType, Type implementedType, ServiceLifetime serviceLifetime, ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (interfaceType == null)
            {
                logger.Error("Interface type is null in method {methodName} while adding service with lifetime {serviceLifetime}.", nameof(AddService), serviceLifetime);
                return;
            }

            if (implementedType == null)
            {
                logger.Error("Implemented type is null in method {methodName} for interface type {interfaceTypeName} while adding service with lifetime {serviceLifetime}.",
                    nameof(AddService),
                    interfaceType.Name,
                    serviceLifetime);
                return;
            }

            if (!implementedType.IsAssignableTo(interfaceType))
            {
                logger.Warning("{implementedTypeName} doesn't implement {interfaceTypeName}. Skipping adding this service to the DI container in {procedureName}.",
                    implementedType.Name,
                    interfaceType.Name,
                    nameof(AddService)
                    );
                return;
            }

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(interfaceType, implementedType);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(interfaceType, implementedType);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(interfaceType, implementedType);
                    break;
            }
        }
    }
}