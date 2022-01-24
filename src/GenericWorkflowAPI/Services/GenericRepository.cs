using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.Services
{
    public class GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity>, IDisposable
        where TEntity : class, IBaseEntity, new()
        where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;
        protected readonly ILogger _logger;
        protected readonly IEntityService<TEntity> _entityService;

        public DbSet<TEntity> DbSet { get; private set; }

        public GenericRepository(TDbContext dbContext, ILogger logger, IEntityService<TEntity> initializeEntityService)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _dbContext = dbContext;
            _logger = logger;
            _entityService = initializeEntityService;
            DbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(long? id, List<string> includePathList, CancellationToken cancellationToken)
        {
            if (id == null)
                return default(TEntity);

            try
            {
                var entitiesQueryable = DbSet.Where(e => !e.IsDeleted);

                if (includePathList != null && includePathList.Count != 0)
                {
                    foreach (var includePath in includePathList)
                    {
                        if (!string.IsNullOrWhiteSpace(includePath))
                            entitiesQueryable = entitiesQueryable.Include(includePath);
                    }
                }

                var entity = await entitiesQueryable.FirstOrDefaultAsync(entity => entity.Id == id.Value, cancellationToken);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(GetByIdAsync),
                    $"{id};{string.Join(",", includePathList)}");

                throw;
            }
        }

        public async Task<List<TEntity>> GetAllAsync(List<string> includePathList, CancellationToken cancellationToken)
        {
            try
            {
                var entitiesQueryable = GetAllQueryable(includePathList);

                return await entitiesQueryable.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(GetAllAsync),
                    string.Join(",", includePathList));

                throw;
            }
        }

        public IQueryable<TEntity> GetAllQueryable(List<string> includePathList)
        {
            try
            {
                var entitiesQueryable = DbSet.Where(e => !e.IsDeleted);

                if (includePathList != null && includePathList.Count != 0)
                {
                    foreach (var includePath in includePathList)
                    {
                        if (!string.IsNullOrWhiteSpace(includePath))
                            entitiesQueryable = entitiesQueryable.Include(includePath);
                    }
                }

                return entitiesQueryable;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(GetAllQueryable),
                    string.Join(",", includePathList));

                throw;
            }
        }

        public async Task AddAsync(TEntity entity, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), $"Cannot add a null entity of type {typeof(TEntity).Name}.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot add an entity of type {typeof(TEntity).Name} using a null user.");

                _entityService.Initialize(entity, user);
                await _dbContext.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(AddAsync),
                    JsonConvert.SerializeObject(entity),
                    user);

                throw;
            }
        }

        public async Task AddRangeAsync(List<TEntity> entitiesList, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (entitiesList == null || entitiesList.Count == 0)
                    throw new ArgumentException(nameof(entitiesList), $"Cannot add a null or empty entity list of type {typeof(TEntity).Name}.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot add an entity list of type {typeof(TEntity).Name} using a null user.");

                foreach (var entity in entitiesList)
                {
                    _entityService.Initialize(entity, user);
                    await _dbContext.AddAsync(entity, cancellationToken);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(AddRangeAsync),
                    JsonConvert.SerializeObject(entitiesList),
                    user);

                throw;
            }
        }

        public async Task UpdateLoadedAsync(TEntity loadedEntity, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (loadedEntity == null)
                    throw new ArgumentNullException(nameof(loadedEntity), $"Cannot update a null entity of type {typeof(TEntity).Name}.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot update an entity of type {typeof(TEntity).Name} using a null user.");

                _entityService.Update(loadedEntity, user);
                _dbContext.Update(loadedEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(UpdateLoadedAsync),
                    JsonConvert.SerializeObject(loadedEntity),
                    user);

                throw;
            }
        }

        public async Task UpdateLoadedAsync(List<TEntity> loadedEntitiesList, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (loadedEntitiesList == null || loadedEntitiesList.Count == 0)
                    throw new ArgumentException(nameof(loadedEntitiesList), $"Cannot update a null or empty entity list of type {typeof(TEntity).Name}.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot update an entity list of type {typeof(TEntity).Name} using a null user.");

                foreach (var loadedEntity in loadedEntitiesList)
                {
                    _entityService.Update(loadedEntity, user);
                    _dbContext.Update(loadedEntity);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(UpdateLoadedAsync),
                    JsonConvert.SerializeObject(loadedEntitiesList),
                    user);

                throw;
            }
        }

        public async Task DeleteAsync(long? id, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), $"Cannot delete an entity of type {typeof(TEntity).Name} using a null id.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot delete an entity of type {typeof(TEntity).Name} using a null user.");

                var entity = await GetByIdAsync(id, new List<string>(), cancellationToken);
                if (entity == null)
                    return;

                _entityService.Delete(entity, user);
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(DeleteAsync),
                    id,
                    user);
                
                throw;
            }
        }

        public async Task DeleteAsync(List<long?> idsList, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (idsList == null || idsList.Count == 0)
                    throw new ArgumentException(nameof(idsList), $"Cannot delete an entity list of type {typeof(TEntity).Name} using a null or empty id.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot delete an entity list of type {typeof(TEntity).Name} using a null user.");

                foreach (var id in idsList)
                {
                    if (id == null)
                        continue;

                    var entity = await GetByIdAsync(id, new List<string>(), cancellationToken);
                    if (entity == null)
                        continue;

                    _entityService.Delete(entity, user);
                    _dbContext.Update(entity);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericRepository<TEntity, TDbContext>).FullName,
                    nameof(DeleteAsync),
                    JsonConvert.SerializeObject(idsList),
                    user);

                throw;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}