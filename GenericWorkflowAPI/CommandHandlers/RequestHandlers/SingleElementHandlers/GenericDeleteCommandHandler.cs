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
    public class GenericDeleteCommandHandler<TEntity, TDto> : IRequestHandler<GenericDeleteRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, ICodeDto, new()
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        private readonly ILogger logger;
        private readonly IGenericCodeRepository<TEntity> repository;
        private readonly IMappingHelper<TEntity, TDto> mappingHelper;

        public GenericDeleteCommandHandler(IGenericCodeRepository<TEntity> _repository, ILogger _logger, IMappingHelper<TEntity, TDto> _mappingHelper)
        {
            repository = _repository;
            logger = _logger;
            mappingHelper = _mappingHelper;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericDeleteRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Code))
                {
                    logger.Error($"Invalid code parameter value {request.Code} for entity {typeof(TEntity).FullName} in method Delete");

                    // Specify the validation error occured
                    return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.Code)}", "Invalid code parameter" } });
                }

                await repository.DeleteAsync(request.Code, cancellationToken);

                return GenericApiResponse<string>.NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{typeof(GenericDeleteCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}({request.Code}) exception");
                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}