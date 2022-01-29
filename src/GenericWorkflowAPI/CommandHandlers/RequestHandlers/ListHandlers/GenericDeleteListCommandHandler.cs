using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using GenericWorkflowAPI.Extensions;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericDeleteListCommandHandler<TEntity, TDto> : IRequestHandler<GenericDeleteListRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, ICodeDto, new()
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        private readonly IGenericCodeRepository<TEntity> _repository;
        private readonly ILogger _logger;

        public GenericDeleteListCommandHandler(IGenericCodeRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericDeleteListRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return GenericApiResponse<string>.Ok();
                if (request == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericDeleteListRequest<TDto>).FullName}");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (request.User == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.User)), $"Cannot handle request of type {typeof(GenericDeleteListRequest<TDto>).FullName} for null user.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.User)}", ValidationConstants.InvalidUserMessage } });
                }
                if (request.Codes == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.Codes)), $"Cannot handle request of type {typeof(GenericDeleteListRequest<TDto>).FullName} for null codes collection.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.Codes)}", ValidationConstants.InvalidCollectionMessage } });
                }

                await _repository.DeleteAsync(request.Codes.ToList(), request.User, cancellationToken);

                return GenericApiResponse<string>.Ok();
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericDeleteCommandHandler<TEntity, TDto>).FullName,
                    nameof(Handle),
                    JsonConvert.SerializeObject(request.Codes),
                    request.User);

                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}