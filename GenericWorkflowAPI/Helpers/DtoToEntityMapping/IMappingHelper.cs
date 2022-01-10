using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;

namespace GenericWorkflowAPI.AutoMapper
{
    public interface IMappingHelper<TEntity, TDto>
        where TEntity : class, IBaseEntity, new()
        where TDto : class, IBaseDto, new()
    {
        TDto MapEntityToDto(TEntity entity);
        Task<TEntity> MapDtoToEntity(TDto dto, CancellationToken cancellationToken);

        List<TDto> MapEntitiesToDtos(List<TEntity> entitiesList);
        Task<List<TEntity>> MapDtosToEntities(List<TDto> dtosList, CancellationToken cancellationToken);
    }
}
