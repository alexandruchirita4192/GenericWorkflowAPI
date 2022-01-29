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
using GenericWorkflowAPI.Extensions;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericCreateListCommandHandler<TEntity, TDto> : IRequestHandler<GenericCreateListRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, new()
        where TEntity : class, IBaseEntity, new()
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly ILogger _logger;
        private readonly IMappingHelper<TEntity, TDto> _mappingHelper;

        public GenericCreateListCommandHandler(IGenericRepository<TEntity> repository, ILogger logger, IMappingHelper<TEntity, TDto> mappingHelper)
        {
            _repository = repository;
            _logger = logger;
            _mappingHelper = mappingHelper;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericCreateListRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return GenericApiResponse<string>.Created(string.Empty);
                if (request == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericCreateListRequest<TDto>).FullName}");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (request.User == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.User)), $"Cannot handle request of type {typeof(GenericCreateListRequest<TDto>).FullName} for null user.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.User)}", ValidationConstants.InvalidUserMessage } });
                }
                if (request.Collection == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.Collection)), $"Cannot handle request of type {typeof(GenericCreateListRequest<TDto>).FullName} for null collection.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.Collection)}", ValidationConstants.InvalidCollectionMessage } });
                }

                var entitiesList = await _mappingHelper.MapDtosToEntities(request.Collection.ToList(), cancellationToken);

                await _repository.AddRangeAsync(entitiesList, request.User, cancellationToken);

                return GenericApiResponse<string>.Created(JsonConvert.SerializeObject(request.Collection, Formatting.Indented));
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericCreateListCommandHandler<TEntity, TDto>).FullName,
                    nameof(Handle),
                    JsonConvert.SerializeObject(request.Collection),
                    request.User);

                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}