using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using GenericWorkflowAPI.Extensions;
using MediatR;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericGetListCommandHandler<TEntity, TDto> : IRequestHandler<GenericGetListRequest<TDto>, GenericApiResponse<List<TDto>>>
        where TDto : class, IBaseDto, new()
        where TEntity : class, IBaseEntity, new()
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GenericGetListCommandHandler(IGenericRepository<TEntity> repository, ILogger logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<GenericApiResponse<List<TDto>>> Handle(GenericGetListRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return GenericApiResponse<List<TDto>>.Ok();
                if (request == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericGetListRequest<TDto>).FullName}");
                    return GenericApiResponse<List<TDto>>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }

                var entitiesList = await _repository.GetAllAsync(request.IncludePathList ?? new List<string>(), cancellationToken);
                if (entitiesList == null || entitiesList.Count == 0)
                    return GenericApiResponse<List<TDto>>.NoContent();

                var dtosList = _mapper.Map<List<TDto>>(entitiesList);

                if (dtosList == null || dtosList.Count == 0)
                {
                    _logger.Error($"{typeof(GenericGetListCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}" +
                        $" wrong DTO list count {dtosList?.Count} different from entities list count {entitiesList?.Count}");
                    return GenericApiResponse<List<TDto>>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
                }

                return GenericApiResponse<List<TDto>>.Ok(dtosList);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericGetListCommandHandler<TEntity, TDto>).FullName,
                    nameof(Handle));

                return GenericApiResponse<List<TDto>>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}