using System.Collections.Generic;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;

namespace GenericWorkflowAPI.Core.AutoMapper.Helpers
{
    public interface IReflectionMappingInfoProvider<TEntity, TDto>
        where TEntity : class, IBaseEntity
        where TDto : class, IBaseDto
    {
        List<ReflectionMappingInfo> GetCacheWithReflectionMappingInfo();
    }
}