using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace GenericWorkflowAPI.Core.AutoMapper.Helpers
{
    public class ReflectionMappingInfoProvider<TEntity, TDto> : IReflectionMappingInfoProvider<TEntity, TDto>
        where TEntity : class, IBaseEntity
        where TDto : class, IBaseDto
    {
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public ReflectionMappingInfoProvider(ILogger logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public List<ReflectionMappingInfo>? GetCacheWithReflectionMappingInfo()
        {
            List<ReflectionMappingInfo>? mappingInfos = null;
            try
            {
                if (!_memoryCache.TryGetValue($"{nameof(mappingInfos)}", out mappingInfos) || mappingInfos == null)
                {
                    mappingInfos = GetReflectionMappingInfoForCurrentEntityAndDto();
                    var cacheEntry = _memoryCache.CreateEntry($"{nameof(mappingInfos)}");
                    cacheEntry.Value = mappingInfos;
                }
                else
                {
                    if (mappingInfos == null)
                    {
                        mappingInfos = GetReflectionMappingInfoForCurrentEntityAndDto();
                        var cacheEntry = _memoryCache.CreateEntry($"{nameof(mappingInfos)}");
                        cacheEntry.Value = mappingInfos;
                    }
                    else if (!mappingInfos.Any(mi => mi.EntityType == typeof(TEntity)))
                    {
                        var mappingInfosForCurrentEntityAndDto = GetReflectionMappingInfoForCurrentEntityAndDto();
                        mappingInfos.AddRange(mappingInfosForCurrentEntityAndDto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{nameof(GetCacheWithReflectionMappingInfo)} exception");
            }
            return mappingInfos;
        }

        protected static List<ReflectionMappingInfo> GetReflectionMappingInfoForCurrentEntityAndDto()
        {
            // Going through properties with reflection to map id property "{InfoType}Id", class property "{InfoType}" and code property "{InfoType}Code"
            var entityType = typeof(TEntity);
            var dtoType = typeof(TDto);

            var entityProperties = entityType.GetProperties();
            var dtoProperties = dtoType.GetProperties();
            var mappedProperties = new List<ReflectionMappingInfo>();

            // Get properties ending with "Id" through reflection (example: "InstanceId" of type "long?")
            foreach (var entityPropertyInfo in entityProperties)
            {
                if (IsValidPropertyInfoId(entityPropertyInfo))
                {
                    var propertyNameWithoutId = entityPropertyInfo.Name
                        .Substring(0, entityPropertyInfo.Name
                            .IndexOf(ReflectionMappingInfo.PropertyNameSuffixId));

                    if (!string.IsNullOrWhiteSpace(propertyNameWithoutId))
                    {
                        mappedProperties.Add(new ReflectionMappingInfo(entityType, propertyNameWithoutId, entityPropertyInfo));
                    }
                }
            }

            // Get entities properties with same name as properties with "Id" at the end but without the "Id"
            // (example: "Instance" of type "WorkflowInstance" has almost the same name as "InstanceId" of type "long?"; those 2 properties are related)
            foreach (var entityPropertyInfo in entityProperties)
            {
                if (!string.IsNullOrWhiteSpace(entityPropertyInfo.Name))
                {
                    var mappedProperty = mappedProperties.FirstOrDefault(
                        mp => mp.BasePropertyName == entityPropertyInfo.Name);
                    if (mappedProperty != null)
                        mappedProperty.EntityPropertyInfoClass = entityPropertyInfo;
                }
            }

            // Get dto properties with same name as properties with "Id" at the end from entities but with "Code" instead
            // (example "InstanceCode" from dto of type "string" has almost the same name as "InstanceId" from entities of type "long?" but has "Code" instead of "Id";
            // those 2 properties are related)
            foreach (var dtoProperty in dtoProperties)
            {
                if (dtoProperty.PropertyType == typeof(string)
                    && !string.IsNullOrWhiteSpace(dtoProperty.Name)
                    && dtoProperty.Name.EndsWith(ReflectionMappingInfo.PropertyNameSuffixCode))
                {
                    var mappedProperty = mappedProperties.FirstOrDefault(mp => mp.DtoPropertyNameCode == dtoProperty.Name);
                    if (mappedProperty != null)
                        mappedProperty.DtoPropertyInfoCode = dtoProperty;
                }
            }

            return mappedProperties;
        }

        /// <summary>
        /// Validates that <paramref name="entityPropertyInfoId"/> is of type <see cref="long?"/>
        ///  with property name ending in <see cref="ReflectionMappingInfo.PropertyNameSuffixId"/>.
        /// </summary>
        private static bool IsValidPropertyInfoId(PropertyInfo entityPropertyInfoId)
        {
            return entityPropertyInfoId != null 
                && entityPropertyInfoId.PropertyType == typeof(long?)
                && !string.IsNullOrWhiteSpace(entityPropertyInfoId.Name)
                && entityPropertyInfoId.Name
                    .EndsWith(ReflectionMappingInfo.PropertyNameSuffixId);
        }
    }
}