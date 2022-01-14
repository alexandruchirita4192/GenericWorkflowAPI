using System;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericUpdateListCommandHandler<TEntity, TDto> : IRequestHandler<GenericUpdateListRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, ICodeDto, new()
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        private readonly IGenericCodeRepository<TEntity> _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GenericUpdateListCommandHandler(IGenericCodeRepository<TEntity> repository, ILogger logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericUpdateListRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(GenericUpdateListRequest<TDto>).FullName}");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (request.User == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.User)), $"Cannot handle request of type {typeof(GenericUpdateListRequest<TDto>).FullName} for null user.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.User)}", ValidationConstants.InvalidUserMessage } });
                }
                if (request.Collection == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.Collection)), $"Cannot handle request of type {typeof(GenericUpdateListRequest<TDto>).FullName} for null collection.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.Collection)}", ValidationConstants.InvalidCollectionMessage } });
                }

                var mappedEntities = _mapper.Map<List<TEntity>>(request.Collection.ToList());

                await _repository.UpdateAsync(mappedEntities, request.User, cancellationToken);

                return GenericApiResponse<string>.Ok();
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericUpdateListCommandHandler<TEntity, TDto>).FullName,
                    nameof(Handle),
                    JsonConvert.SerializeObject(request.Collection),
                    request.User);

                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}