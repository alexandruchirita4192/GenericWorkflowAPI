using System;
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
    public class GenericCreateCommandHandler<TEntity, TDto> : IRequestHandler<GenericCreateRequest<TDto>, GenericApiResponse<string>>
        where TDto : class, IBaseDto, new()
        where TEntity : class, IBaseEntity, new()
    {
        private readonly ILogger logger;
        private readonly IGenericRepository<TEntity> repository;
        private readonly IMappingHelper<TEntity, TDto> mappingHelper;

        public GenericCreateCommandHandler(IGenericRepository<TEntity> _repository, ILogger _logger, IMappingHelper<TEntity, TDto> _mappingHelper)
        {
            repository = _repository;
            logger = _logger;
            mappingHelper = _mappingHelper;
        }

        public async Task<GenericApiResponse<string>> Handle(GenericCreateRequest<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await mappingHelper.MapDtoToEntity(request.Item, cancellationToken);

                await repository.AddAsync(entity, cancellationToken);

                return GenericApiResponse<string>.Created(JsonConvert.SerializeObject(request.Item, Formatting.Indented));
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{typeof(GenericCreateCommandHandler<TEntity, TDto>).FullName}.{nameof(Handle)}({JsonConvert.SerializeObject(request.Item)}) exception");
                return GenericApiResponse<string>.Problem(ValidationConstants.GenericValidationMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}