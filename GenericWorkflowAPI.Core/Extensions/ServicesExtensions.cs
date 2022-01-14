using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GenericWorkflowAPI.Core.Extensions
{
    public static class ServicesExtensions
    {
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

        public static void RegisterEncodingProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
            string procedureName)
        {
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

                    if (!implementedType.IsAssignableTo(interfaceType))
                    {
                        logger.Warning("{implementedTypeName} doesn't implement {interfaceTypeName}. Skipping adding this service to the DI container in {procedureName}.",
                            implementedType.Name,
                            interfaceType.Name,
                            procedureName ?? nameof(AddServices)
                            );
                        continue;
                    }

                    services.AddScoped(interfaceType, implementedType);
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
    }
}