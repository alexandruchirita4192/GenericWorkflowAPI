using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GenericDeleteListCommandHandler<TEntity, TDto> : IRequestHandler<GenericDeleteListRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, ICodeDto, new()
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        private readonly ILogger logger;
        private readonly IGenericCodeRepository<TEntity> repository;
        private readonly IMappingHelper<TEntity, TDto> mappingHelper;

        public GenericDeleteListCommandHandler(IGenericCodeRepository<TEntity> _repository, ILogger _logger, IMappingHelper<TEntity, TDto> _mappingHelper)
        {
            repository = _repository;
            logger = _logger;
            mappingHelper = _mappingHelper;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericDeleteListRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericDeleteListRequest<TDto>).FullName}");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (request.User == null)
                {
                    logger.Error(new ArgumentNullException(nameof(request.User)), $"Cannot handle request of type {typeof(GenericDeleteListRequest<TDto>).FullName} for null user.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.User)}", ValidationConstants.InvalidUserMessage } });
                }

                await repository.DeleteAsync(request.Codes.ToList(), request.User, cancellationToken);

                return GenericApiResponse<string>.Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{typeof(GenericDeleteCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}({JsonConvert.SerializeObject(request.Codes)}) exception");
                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}