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
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericGetCommandHandler<TEntity, TDto> : IRequestHandler<GenericGetRequest<TDto>, GenericApiResponse<TDto>>
        where TDto : class, IBaseDto, ICodeDto, new()
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        private readonly IGenericCodeRepository<TEntity> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GenericGetCommandHandler(IGenericCodeRepository<TEntity> repository, ILogger logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<GenericApiResponse<TDto>> Handle(GenericGetRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return GenericApiResponse<TDto>.Ok();
                if (request == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericGetRequest<TDto>).FullName}");
                    return GenericApiResponse<TDto>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    return InternalInvalidCode(request);
                }

                var entity = await _repository.GetByCodeAsync(request.Code, request.IncludePathList ?? new List<string>(), cancellationToken);
                if (entity == null)
                {
                    return InternalInvalidCode(request);
                }

                var dto = _mapper.Map<TDto>(entity);
                if (dto == null)
                {
                    _logger.Error($"Invalid dto received while mapping entity ({JsonConvert.SerializeObject(entity)}) to dto (null); request code is {request.Code}");
                    return GenericApiResponse<TDto>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
                }

                return GenericApiResponse<TDto>.Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericGetCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}({request.Code}) exception");
                return GenericApiResponse<TDto>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }

        private GenericApiResponse<TDto> InternalInvalidCode(GenericGetRequest<TDto> request)
        {
            _logger.Error($"Invalid code {request.Code}");
            return GenericApiResponse<TDto>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.Code)}", ValidationConstants.InvalidCodeMessage } });
        }
    }
}