using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.Entities;
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
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(GetByIdAsync)}({id}) function error");
                throw;
            }
        }

        public async Task<List<TEntity>> GetAllAsync(List<string> includePathList, CancellationToken cancellationToken)
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

                return await entitiesQueryable.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(GetAllAsync)}({string.Join(",", includePathList)}) function error");
                throw;
            }
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity == null)
                    return;

                _entityService.Initialize(entity);
                await _dbContext.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(AddAsync)}({JsonConvert.SerializeObject(entity)}) function error");
                throw;
            }
        }

        public async Task AddRangeAsync(List<TEntity> entitiesList, CancellationToken cancellationToken)
        {
            try
            {
                if (entitiesList == null || entitiesList.Count == 0)
                    return;

                foreach (var entity in entitiesList)
                {
                    _entityService.Initialize(entity);
                    await _dbContext.AddAsync(entity, cancellationToken);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(AddRangeAsync)}({JsonConvert.SerializeObject(entitiesList)}) function error");
                throw;
            }
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity == null)
                    return;

                _entityService.Update(entity);
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(UpdateAsync)}({JsonConvert.SerializeObject(entity)}) function error");
                throw;
            }
        }

        public async Task UpdateAsync(List<TEntity> entitiesList, CancellationToken cancellationToken)
        {
            try
            {
                if (entitiesList == null || entitiesList.Count == 0)
                    return;

                foreach (var entity in entitiesList)
                {
                    _entityService.Update(entity);
                    _dbContext.Update(entity);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(UpdateAsync)}({JsonConvert.SerializeObject(entitiesList)}) function error");
                throw;
            }
        }

        public async Task DeleteAsync(long? id, CancellationToken cancellationToken)
        {
            try
            {
                if (id == null)
                    return;

                var entity = await GetByIdAsync(id, new List<string>(), cancellationToken);
                if (entity == null)
                    return;

                _entityService.Delete(entity);
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(DeleteAsync)}({id}) function error");
                throw;
            }
        }

        public async Task DeleteAsync(List<long?> idsList, CancellationToken cancellationToken)
        {
            try
            {
                if (idsList == null || idsList.Count == 0)
                    return;

                foreach (var id in idsList)
                {
                    if (id == null)
                        continue;

                    var entity = await GetByIdAsync(id, new List<string>(), cancellationToken);
                    if (entity == null)
                        continue;

                    _entityService.Delete(entity);
                    _dbContext.Update(entity);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(DeleteAsync)}({JsonConvert.SerializeObject(idsList)}) function error");
                throw;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}