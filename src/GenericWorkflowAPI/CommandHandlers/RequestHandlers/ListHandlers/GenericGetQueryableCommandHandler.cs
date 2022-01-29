using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using MediatR;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericGetQueryableCommandHandler<TEntity, TDto> : IRequestHandler<GenericGetQueryableRequest<TDto>, IQueryable<TDto>>
        where TDto : class, IBaseDto, new()
        where TEntity : class, IBaseEntity, new()
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericGetQueryableCommandHandler(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handle queryable requests
        /// </summary>
        /// <remarks>This handler has less error-handling because it needs to return IQueryable<TDto> and a generic error-handling has been created.</remarks>
        public async Task<IQueryable<TDto>> Handle(GenericGetQueryableRequest<TDto> request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return new List<TDto>().AsQueryable();
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var entitiesQueryable = await Task.Run(() => _repository.GetAllQueryable(request.IncludePathList ?? new List<string>()));

            var dtosQueryable = entitiesQueryable.ProjectTo<TDto>(_mapper.ConfigurationProvider, request.QueryOptions);

            return dtosQueryable;
        }
    }
}