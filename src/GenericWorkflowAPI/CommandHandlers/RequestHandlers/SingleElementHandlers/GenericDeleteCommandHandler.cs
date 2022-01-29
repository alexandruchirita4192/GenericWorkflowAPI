using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
    public class GenericDeleteCommandHandler<TEntity, TDto> : IRequestHandler<GenericDeleteRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, ICodeDto, new()
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        private readonly IGenericCodeRepository<TEntity> _repository;
        private readonly ILogger _logger;

        public GenericDeleteCommandHandler(IGenericCodeRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericDeleteRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return GenericApiResponse<string>.Ok();
                if (request == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericDeleteRequest<TDto>).FullName}");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (request.User == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.User)), $"Cannot handle request of type {typeof(GenericDeleteRequest<TDto>).FullName} for null user.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.User)}", ValidationConstants.InvalidUserMessage } });
                }
                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    _logger.Error($"Invalid code parameter value {request.Code} for entity {typeof(TEntity).FullName} in method Delete");

                    // Specify the validation error occured
                    return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.Code)}", ValidationConstants.InvalidCodeMessage } });
                }

                await _repository.DeleteAsync(request.Code, request.User, cancellationToken);

                return GenericApiResponse<string>.Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericDeleteCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}({request.Code}) exception");
                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}