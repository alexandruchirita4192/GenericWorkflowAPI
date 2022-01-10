using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.AutoMapper;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericGetListCommandHandler<TEntity, TDto> : IRequestHandler<GenericGetListRequest<TDto>, GenericApiResponse<List<TDto>>>
        where TDto : class, IBaseDto, new()
        where TEntity : class, IBaseEntity, new()
    {
        private readonly ILogger logger;
        private readonly IGenericRepository<TEntity> repository;
        private readonly IMappingHelper<TEntity, TDto> mappingHelper;

        public GenericGetListCommandHandler(IGenericRepository<TEntity> _repository, ILogger _logger, IMappingHelper<TEntity, TDto> _mappingHelper)
        {
            repository = _repository;
            logger = _logger;
            mappingHelper = _mappingHelper;
        }

        public IGenericRepository<TEntity> Repository => repository;

        public async Task<GenericApiResponse<List<TDto>>> Handle(GenericGetListRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesList = await Repository.GetAllAsync(request.IncludePathList ?? new List<string>(), cancellationToken);
                if (entitiesList == null || entitiesList.Count == 0)
                    return GenericApiResponse<List<TDto>>.NoContent();

                var dtosList = mappingHelper.MapEntitiesToDtos(entitiesList);
                if (dtosList == null || dtosList.Count == 0)
                {
                    logger.Error($"{typeof(GenericGetListCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}" +
                        $" wrong DTO list count {dtosList?.Count} different from entities list count {entitiesList?.Count}");
                    return GenericApiResponse<List<TDto>>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
                }

                return GenericApiResponse<List<TDto>>.Ok(dtosList);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{typeof(GenericGetListCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}() exception");
                return GenericApiResponse<List<TDto>>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}