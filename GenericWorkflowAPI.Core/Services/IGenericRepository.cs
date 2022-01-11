using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenericWorkflowAPI.Core.Services
{
    public interface IGenericRepository<TEntity> : IDisposable
        where TEntity : class, IBaseEntity, new()
    {
        public DbSet<TEntity> DbSet { get; }
        public Task<TEntity> GetByIdAsync(long? id, List<string> includePathList, CancellationToken cancellationToken);

        public Task<List<TEntity>> GetAllAsync(List<string> includePathList, CancellationToken cancellationToken);

        public Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        public Task AddRangeAsync(List<TEntity> entitiesList, CancellationToken cancellationToken);

        public Task UpdateLoadedAsync(TEntity loadedEntity, CancellationToken cancellationToken);

        public Task UpdateLoadedAsync(List<TEntity> loadedEntitiesList, CancellationToken cancellationToken);

        public Task DeleteAsync(long? id, CancellationToken cancellationToken);

        public Task DeleteAsync(List<long?> idsList, CancellationToken cancellationToken);
    }
}