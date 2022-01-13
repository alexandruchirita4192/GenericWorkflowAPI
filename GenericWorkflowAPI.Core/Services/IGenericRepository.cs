using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain;
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

        public IQueryable<TEntity> GetAllQueryable(List<string> includePathList);

        public Task AddAsync(TEntity entity, IdentityUser user, CancellationToken cancellationToken);

        public Task AddRangeAsync(List<TEntity> entitiesList, IdentityUser user, CancellationToken cancellationToken);

        public Task UpdateLoadedAsync(TEntity loadedEntity, IdentityUser user, CancellationToken cancellationToken);

        public Task UpdateLoadedAsync(List<TEntity> loadedEntitiesList, IdentityUser user, CancellationToken cancellationToken);

        public Task DeleteAsync(long? id, IdentityUser user, CancellationToken cancellationToken);

        public Task DeleteAsync(List<long?> idsList, IdentityUser user, CancellationToken cancellationToken);
    }
}