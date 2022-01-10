using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GenericWorkflowAPI.Core.AutoMapper;
using GenericWorkflowAPI.Core.AutoMapper.Helpers;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Database;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GenericWorkflowAPI.AutoMapper
{
    public partial class MappingHelper<TEntity, TDto> : IMappingHelper<TEntity, TDto>
        where TEntity : class, IBaseEntity, new()
        where TDto : class, IBaseDto, new()
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IReflectionMappingInfoProvider<TEntity, TDto> _reflectionMappingInfoProvider;
        private readonly IServiceProvider _serviceProvider;

        private readonly List<ReflectionMappingInfo> MappingInfos;

        public MappingHelper(ILogger logger, IMapper mapper, IReflectionMappingInfoProvider<TEntity, TDto> reflectionMappingInfoProvider, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _logger = logger;
            _reflectionMappingInfoProvider = reflectionMappingInfoProvider;
            _serviceProvider = serviceProvider;

            MappingInfos = _reflectionMappingInfoProvider.GetCacheWithReflectionMappingInfo();
        }

        public TDto MapEntityToDto(TEntity entity)
        {
            // This mapping might not do anything at all actually for some TEntity
            var dto = _mapper.Map<TEntity, TDto>(entity);

            // Parse all reflection property infos
            foreach (var reflectionMappedInfo in MappingInfos)
            {
                // Skip incomplete mapping properties
                if (string.IsNullOrWhiteSpace(reflectionMappedInfo.BasePropertyName)
                || reflectionMappedInfo.EntityPropertyInfoClass == null
                || reflectionMappedInfo.DtoPropertyInfoCode == null)
                {
                    _logger.Debug($"Incomplete mapping types: BasePropertyName={reflectionMappedInfo.BasePropertyName}\n" +
                        $"EntityPropertyInfoClass?.Name={reflectionMappedInfo.EntityPropertyInfoClass?.Name} of type {reflectionMappedInfo.EntityPropertyInfoClass?.PropertyType}\n" +
                        $"DtoPropertyInfoCode?.Name={reflectionMappedInfo.DtoPropertyInfoCode?.Name} of type {reflectionMappedInfo.DtoPropertyInfoCode?.PropertyType}");
                    continue;
                }

                var classPropertyValue = reflectionMappedInfo.EntityPropertyInfoClass.GetValue(entity, null) as ICodeEntity;

                // If the [classPropertyValue] doesn't have a "[classPropertyValue].Code" property (defined by ICodeEntity)
                // then setting the "Code" property for Dto won't work
                if (classPropertyValue == null)
                    continue;

                var codeValue = classPropertyValue.Code;
                if (string.IsNullOrWhiteSpace(codeValue))
                    continue;

                // Set the value of the TDto property "{}" based on the value of the TEntity
                reflectionMappedInfo.DtoPropertyInfoCode.SetValue(dto, codeValue, null);
            }

            return dto;
        }

        public async Task<TEntity> MapDtoToEntity(TDto dto, CancellationToken cancellationToken)
        {
            // This mapping might not do anything at all actually for some TEntity
            var entity = _mapper.Map<TDto, TEntity>(dto);

            // TEntity can only be filled by a proper load from database if needed (by code if the TDto has one)

            var applicationDbContextType = typeof(ApplicationDbContext);

            // Parse all reflection property infos
            foreach (var reflectionMappedInfo in MappingInfos)
            {
                try
                {
                    // Skip incomplete mapping properties
                    if (string.IsNullOrWhiteSpace(reflectionMappedInfo.BasePropertyName)
                    || reflectionMappedInfo.EntityPropertyInfoClass == null
                    || reflectionMappedInfo.DtoPropertyInfoCode == null)
                    {
                        _logger.Debug($"Incomplete mapping types: BasePropertyName={reflectionMappedInfo.BasePropertyName}\n" +
                            $"EntityPropertyInfoClass?.Name={reflectionMappedInfo.EntityPropertyInfoClass?.Name} of type {reflectionMappedInfo.EntityPropertyInfoClass?.PropertyType}\n" +
                            $"DtoPropertyInfoCode?.Name={reflectionMappedInfo.DtoPropertyInfoCode?.Name} of type {reflectionMappedInfo.DtoPropertyInfoCode?.PropertyType}");
                        continue;
                    }

                    // Set the value of the TDto property "{}" based on the value of the TEntity
                    var codeValue = reflectionMappedInfo.DtoPropertyInfoCode.GetValue(dto, null) as string;
                    if (string.IsNullOrWhiteSpace(codeValue))
                    {
                        _logger.Warning($"Invalid code value '{codeValue}'.");
                        continue;
                    }

                    var entityTypeFromProperty = reflectionMappedInfo.EntityPropertyInfoClass.PropertyType;
                    if (!entityTypeFromProperty.IsAssignableTo(typeof(ICodeEntity)))
                    {
                        _logger.Warning($"Invalid entity type {entityTypeFromProperty.Name} which cannot be assigned to {typeof(ICodeEntity).Name}");
                        continue;
                    }

                    var codeRepositoryType = typeof(GenericCodeRepository<,>).MakeGenericType(entityTypeFromProperty, applicationDbContextType);
                    var codeRepositoryInstance = ActivatorUtilities.CreateInstance(_serviceProvider, codeRepositoryType) as IGenericCodeRepository;
                    if (codeRepositoryInstance == null)
                    {
                        _logger.Warning("IGenericCodeRepository of type {codeRepositoryTypeName} is null.",
                            codeRepositoryType.Name);
                        continue;
                    }

                    var childEntity = await codeRepositoryInstance.GetInterfaceTypeByCodeAsync(codeValue, new List<string>(), cancellationToken);
                    if (childEntity == null)
                    {
                        _logger.Warning("Child entity retrived by IGenericCodeRepository of type {entityTypeName} is null.",
                            entityTypeFromProperty.Name);
                        continue;
                    }

                    // Set the value of the TDto property "{}" based on the value of the TEntity
                    reflectionMappedInfo.EntityPropertyInfoId.SetValue(entity, childEntity.Id, null);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, "MapDtoToEntity exception occured in foreach for item {@reflectionMappedInfo}", reflectionMappedInfo);
                }
            }

            return entity;
        }

        public List<TDto> MapEntitiesToDtos(List<TEntity> entitiesList)
        {
            if (entitiesList == null || entitiesList.Count == 0)
                return default(List<TDto>);

            var dtosList = new List<TDto>();

            foreach (var entity in entitiesList)
            {
                var dto = MapEntityToDto(entity);
                if (dto != null)
                    dtosList.Add(dto);
            }

            return dtosList;
        }

        public async Task<List<TEntity>> MapDtosToEntities(List<TDto> dtosList, CancellationToken cancellationToken)
        {
            if (dtosList == null || dtosList.Count == 0)
                return default(List<TEntity>);

            var entitiesList = new List<TEntity>();

            foreach (var dto in dtosList)
            {
                var entity = await MapDtoToEntity(dto, cancellationToken);
                if (entity != null)
                    entitiesList.Add(entity);
            }

            return entitiesList;
        }

    }
}