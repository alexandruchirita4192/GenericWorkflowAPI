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
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericGetCommandHandler<TEntity, TDto> : IRequestHandler<GenericGetRequest<TDto>, GenericApiResponse<TDto>>
        where TDto : class, IBaseDto, ICodeDto, new()
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        private readonly ILogger logger;
        private readonly IGenericCodeRepository<TEntity> repository;
        private readonly IMappingHelper<TEntity, TDto> mappingHelper;

        public GenericGetCommandHandler(IGenericCodeRepository<TEntity> _repository, ILogger _logger, IMappingHelper<TEntity, TDto> _mappingHelper)
        {
            repository = _repository;
            logger = _logger;
            mappingHelper = _mappingHelper;
        }

        public async Task<GenericApiResponse<TDto>> Handle(GenericGetRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.Code))
                {
                    return InternalInvalidCode(request);
                }

                var entity = await repository.GetByCodeAsync(request.Code, request.IncludePathList ?? new List<string>(), cancellationToken);
                if (entity == null)
                {
                    return InternalInvalidCode(request);
                }

                var dto = mappingHelper.MapEntityToDto(entity);
                if (dto == null)
                {
                    logger.Error($"Invalid dto received while mapping entity ({JsonConvert.SerializeObject(entity)}) to dto (null); request code is {request.Code}");
                    return GenericApiResponse<TDto>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
                }

                return GenericApiResponse<TDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{typeof(GenericCreateCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}({request.Code}) exception");
                return GenericApiResponse<TDto>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }

        private GenericApiResponse<TDto> InternalInvalidCode(GenericGetRequest<TDto> request)
        {
            logger.Error($"Invalid code {request.Code}");
            return GenericApiResponse<TDto>.Problem(ValidationConstants.InvalidCodeMessage, HttpStatusCode.Conflict);
        }
    }
}