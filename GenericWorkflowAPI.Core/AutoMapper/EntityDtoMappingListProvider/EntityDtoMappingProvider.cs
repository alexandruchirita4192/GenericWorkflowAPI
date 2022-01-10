using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Serilog;

namespace GenericWorkflowAPI.Core.AutoMapper.Helpers
{
    public class EntityDtoMappingProvider : IEntityDtoMappingProvider
    {
        private ILogger _logger { get; }
        
        public EntityDtoMappingProvider(ILogger logger)
        {
            _logger = logger;
        }

        public Dictionary<Type, Type> GetEntityDtoMapping(Assembly assembly)
        {
            var mapping = new Dictionary<Type, Type>();

            if (assembly == null)
            {
                _logger.Error("Missing assembly in method {methodName}", nameof(GetEntityDtoMapping));
                return mapping;
            }

            var publicClasses = assembly.GetTypes().Where(t=>t.IsClass && !t.IsAbstract && t.IsPublic).ToList();
            var entities = publicClasses.Where(t => t.IsAssignableTo(typeof(IIdEntity))).ToList();
            var dtos = publicClasses.Where(t => t.IsAssignableTo(typeof(IBaseDto))).ToList();

            if (entities.Count != dtos.Count)
            {
                _logger.Warning("Number of entities ({entities}) is different from number of dtos ({dtos})", entities.Count, dtos.Count);
            }

            // Just fill the dictionary with it's keys (entities)
            foreach(var entity in entities)
            {
                mapping[entity] = entity;
            }

            // Map entities to dtos
            foreach(var dto in dtos)
            {
                var dtoTypeName = dto.Name;
                var item = mapping
                    .Select(pair=> (KeyValuePair<Type,Type>?)pair)
                    .FirstOrDefault(pair =>
                {
                    return string.Compare($"{pair?.Key.Name}Dto", dtoTypeName) == 0;
                });

                if (item != null)
                {
                    mapping[item.Value.Key] = dto;
                }
                else
                {
                    _logger.Warning("{dtoTypeName} doesn't have an entity match", dtoTypeName);
                }
            }

            // Remove invalid mapping elements
            var invalidElements = new List<KeyValuePair<Type, Type>>();
            foreach(var item in mapping)
            {
                if (item.Key == item.Value)
                {
                    _logger.Warning("Entity {entityTypeName} doesn't have a matching dto remaining matched to itself. Removing from mapping collection.", item.Key.Name);
                    invalidElements.Add(item);
                }

                if ($"{item.Key.Name}Dto" != item.Value.Name)
                {
                    _logger.Warning("Entity {entityTypeName} doesn't match properly with {dtoTypeName}. Removing from mapping collection.", item.Key.Name, item.Value.Name);
                    invalidElements.Add(item);
                }
            }

            foreach(var invalidElement in invalidElements)
            {
                mapping.Remove(invalidElement.Key);
            }

            if (mapping.Count == 0)
            {
                _logger.Error("No TEntity mapped to TDto from {methodName}", nameof(GetEntityDtoMapping));
            }

            return mapping;
        }
    }
}