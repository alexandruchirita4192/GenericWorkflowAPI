using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Serilog;

namespace GenericWorkflowAPI.Core.AutoMapper
{
    public class EntityDtoMappingProvider : IEntityDtoMappingProvider
    {
        private ILogger _logger { get; }

        /// <summary>
        /// Keeps entity to dto mappings per each assembly in a cache, using a concurrent dictionary to allow concurrency
        /// </summary>
        /// <remarks>This service must be used as a singleton in order to have only one cache instance having the same behaviour as a static.</remarks>
        private ConcurrentDictionary<Assembly, Dictionary<Type, Type>> entityToDtoMappings;

        public EntityDtoMappingProvider(ILogger logger)
        {
            _logger = logger;
            entityToDtoMappings = new ConcurrentDictionary<Assembly, Dictionary<Type, Type>>();
        }

        public EntityDtoMapping GetEntityDtoMapping(Assembly assembly)
        {
            if (assembly == null)
            {
                _logger.Error("Missing assembly in method {methodName}", nameof(GetEntityDtoMapping));
                return new EntityDtoMapping(assembly);
            }

            if (entityToDtoMappings.Count != 0 && entityToDtoMappings.ContainsKey(assembly))
                return new EntityDtoMapping(entityToDtoMappings, assembly);

            var publicClasses = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsPublic).ToList();
            var entities = publicClasses.Where(t => t.IsAssignableTo(typeof(IIdEntity))).ToList();
            var dtos = publicClasses.Where(t => t.IsAssignableTo(typeof(IBaseDto))).ToList();

            if (entities.Count != dtos.Count)
            {
                _logger.Warning("Number of entities ({entities}) is different from number of dtos ({dtos})", entities.Count, dtos.Count);
            }

            if (!entityToDtoMappings.TryAdd(assembly, new Dictionary<Type, Type>()))
            {
                _logger.Error("Failed to add the assembly {assemblyFullName} to the dictionary.", assembly.FullName);
                return new EntityDtoMapping(assembly);
            }

            // Just fill the dictionary with it's keys (entities)
            foreach (var entity in entities)
            {
                entityToDtoMappings[assembly][entity] = entity;
            }

            // Map entities to dtos
            foreach (var dto in dtos)
            {
                var dtoTypeName = dto.Name;
                var item = entityToDtoMappings[assembly]
                    .Select(pair => (KeyValuePair<Type, Type>?)pair)
                    .FirstOrDefault(pair =>
                {
                    return string.Compare($"{pair?.Key.Name}Dto", dtoTypeName) == 0;
                });

                if (item != null)
                {
                    entityToDtoMappings[assembly][item.Value.Key] = dto;
                }
                else
                {
                    _logger.Warning("{dtoTypeName} doesn't have an entity match", dtoTypeName);
                }
            }

            //// Remove invalid mapping elements
            //var invalidElements = new List<KeyValuePair<Type, Type>>();
            //foreach(var item in entityToDtoMappings[assembly])
            //{
            //    if (item.Key == item.Value)
            //    {
            //        _logger.Warning("Entity {entityTypeName} doesn't have a matching dto remaining matched to itself. Removing from mapping collection.", item.Key.Name);
            //        invalidElements.Add(item);
            //        continue;
            //    }

            //    if ($"{item.Key.Name}Dto" != item.Value.Name)
            //    {
            //        _logger.Warning("Entity {entityTypeName} doesn't match properly with {dtoTypeName}. Removing from mapping collection.", item.Key.Name, item.Value.Name);
            //        invalidElements.Add(item);
            //        continue;
            //    }
            //}

            //foreach(var invalidElement in invalidElements)
            //{
            //    if (!entityToDtoMappings[assembly].Remove(invalidElement.Key))
            //    {
            //        _logger.Error("Couldn't remove invalid element {@invalidElement} in {methodName}",
            //            invalidElement,
            //            nameof(GetEntityDtoMapping));
            //    }
            //}

            if (entityToDtoMappings[assembly].Count == 0)
            {
                _logger.Error("No TEntity mapped to TDto from {methodName}", nameof(GetEntityDtoMapping));
            }

            return new EntityDtoMapping(entityToDtoMappings, assembly);
        }
    }
}