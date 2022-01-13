using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using MediatR;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericGetQueryableCommandHandler<TEntity, TDto> : IRequestHandler<GenericGetQueryableRequest<TDto>, IQueryable<TDto>>
        where TDto : class, IBaseDto, new()
        where TEntity : class, IBaseEntity, new()
    {
        private readonly ILogger logger;
        private readonly IGenericRepository<TEntity> repository;

        public GenericGetQueryableCommandHandler(IGenericRepository<TEntity> _repository, ILogger _logger)
        {
            repository = _repository;
            logger = _logger;
        }

        /// <summary>
        /// Handle queryable requests
        /// </summary>
        /// <remarks>This handler has less error-handling because it needs to return IQueryable<TDto> and a generic error-handling has been created.</remarks>
        public async Task<IQueryable<TDto>> Handle(GenericGetQueryableRequest<TDto> request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var entitiesQueryable = repository.GetAllQueryable(request.IncludePathList ?? new List<string>());

            var dtosQueryable = entitiesQueryable.ProjectTo<TDto>(null, request.QueryOptions);

            return dtosQueryable;
        }
    }
}