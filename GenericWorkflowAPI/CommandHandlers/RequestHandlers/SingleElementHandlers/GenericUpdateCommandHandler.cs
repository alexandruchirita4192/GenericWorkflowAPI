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
    public class GenericUpdateCommandHandler<TEntity, TDto> : IRequestHandler<GenericUpdateRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, ICodeDto, new()
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        private readonly ILogger _logger;
        private readonly IGenericCodeRepository<TEntity> _repository;
        private readonly IMappingHelper<TEntity, TDto> _mappingHelper;

        public GenericUpdateCommandHandler(IGenericCodeRepository<TEntity> repository, ILogger logger, IMappingHelper<TEntity, TDto> mappingHelper)
        {
            _repository = repository;
            _logger = logger;
            _mappingHelper = mappingHelper;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericUpdateRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericUpdateRequest<TDto>).FullName}");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (request.User == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.User)), $"Cannot handle request of type {typeof(GenericUpdateRequest<TDto>).FullName} for null user.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.User)}", ValidationConstants.InvalidUserMessage } });
                }

                var mappedEntity = await _mappingHelper.MapDtoToEntity(request.Item, cancellationToken);

                await _repository.UpdateAsync(mappedEntity, request.User, cancellationToken);

                return GenericApiResponse<string>.Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericUpdateCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}({JsonConvert.SerializeObject(request.Item)}) exception");
                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}