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
    public class GenericCreateListCommandHandler<TEntity, TDto> : IRequestHandler<GenericCreateListRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, new()
        where TEntity : class, IBaseEntity, new()
    {
        private readonly ILogger logger;
        private readonly IGenericRepository<TEntity> repository;
        private readonly IMappingHelper<TEntity, TDto> mappingHelper;

        public GenericCreateListCommandHandler(IGenericRepository<TEntity> _repository, ILogger _logger, IMappingHelper<TEntity, TDto> _mappingHelper)
        {
            repository = _repository;
            logger = _logger;
            mappingHelper = _mappingHelper;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericCreateListRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericCreateListRequest<TDto>).FullName}");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (request.User == null)
                {
                    logger.Error(new ArgumentNullException(nameof(request.User)), $"Cannot handle request of type {typeof(GenericCreateListRequest<TDto>).FullName} for null user.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.User)}", ValidationConstants.InvalidUserMessage } });
                }

                var entitiesList = await mappingHelper.MapDtosToEntities(request.Collection.ToList(), cancellationToken);

                await repository.AddRangeAsync(entitiesList, request.User, cancellationToken);

                return GenericApiResponse<string>.Created(JsonConvert.SerializeObject(request.Collection, Formatting.Indented));
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{typeof(GenericCreateListCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}({JsonConvert.SerializeObject(request.Collection)}) exception");
                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}