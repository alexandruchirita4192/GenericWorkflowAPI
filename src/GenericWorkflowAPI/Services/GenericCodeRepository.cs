using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Extensions;
using GenericWorkflowAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.Services
{
    public class GenericCodeRepository<TEntity, TDbContext> : GenericRepository<TEntity, TDbContext>, IGenericCodeRepository<TEntity>
        where TEntity : class, IBaseEntity, ICodeEntity, new()
        where TDbContext : DbContext
    {
        public GenericCodeRepository(TDbContext dbContext, ILogger logger, IEntityService<TEntity> entityService) 
            : base(dbContext, logger, entityService)
        {
        }

        public async Task<ICodeEntity> GetInterfaceTypeByCodeAsync(string code, List<string> includePathList, CancellationToken cancellationToken)
        {
            return await GetByCodeAsync(code, includePathList, cancellationToken);
        }

        public async Task<TEntity> GetByCodeAsync(string code, List<string> includePathList, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(code))
                return default(TEntity);

            try
            {
                var entitiesQueryable = DbSet.Where(entity => !entity.IsDeleted);

                if (includePathList != null && includePathList.Count != 0)
                {
                    foreach (var includePath in includePathList)
                    {
                        if (!string.IsNullOrWhiteSpace(includePath))
                            entitiesQueryable = entitiesQueryable.Include(includePath);
                    }
                }

                var entity = await entitiesQueryable.FirstOrDefaultAsync(entity => entity.Code == code, cancellationToken);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericCodeRepository<TEntity, TDbContext>).FullName,
                    nameof(GetByCodeAsync),
                    $"{code};{string.Join(",", includePathList)}");

                throw;
            }
        }

        public async Task<List<TEntity>> GetByCodeListAsync(List<string> codesList, List<string> includePathList, CancellationToken cancellationToken)
        {
            if (codesList == null || codesList.Count == 0)
                return default(List<TEntity>);

            try
            {
                var entitiesQueryable = DbSet.Where(entity => !entity.IsDeleted);

                if (includePathList != null && includePathList.Count != 0)
                {
                    foreach (var includePath in includePathList)
                    {
                        if (!string.IsNullOrWhiteSpace(includePath))
                            entitiesQueryable = entitiesQueryable.Include(includePath);
                    }
                }

                return await entitiesQueryable.Where(entity => codesList.Contains(entity.Code)).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericCodeRepository<TEntity, TDbContext>).FullName,
                    nameof(GetByCodeListAsync),
                    $"{JsonConvert.SerializeObject(codesList)};{string.Join(",", includePathList)}");

                throw;
            }
        }

        public async Task UpdateAsync(TEntity entity, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), $"Cannot update a null entity.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot update an entity with a null user.");

                // TODO: Maybe improve performance somehow because the entity is reloaded from database based on code..
                var loadedEntity = await GetByCodeAsync(entity.Code, new List<string>(), cancellationToken);
                
                // Copy all properties through reflection. Slow.. TODO: Make it faster maybe? At least properties cached or something...
                var savedId = loadedEntity.Id;
                entity.CopyProperties(loadedEntity);
                loadedEntity.Id = savedId; // Load all except the Id

                _entityService.Update(loadedEntity, user);
                _dbContext.Update(loadedEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericCodeRepository<TEntity, TDbContext>).FullName,
                    nameof(UpdateAsync),
                    JsonConvert.SerializeObject(entity),
                    user);

                throw;
            }
        }

        public async Task UpdateAsync(List<TEntity> entitiesList, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (entitiesList == null || entitiesList.Count == 0)
                    throw new ArgumentException(nameof(entitiesList), $"Cannot update a null entity list or with count zero.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot update an entity list with a null user.");

                foreach (var entity in entitiesList)
                {
                    // TODO: Maybe improve performance somehow because the entity is reloaded from database based on code..
                    var loadedEntity = await GetByCodeAsync(entity.Code, new List<string>(), cancellationToken);

                    // Copy all properties through reflection. Slow.. TODO: Make it faster maybe? At least properties cached or something...
                    var savedId = loadedEntity.Id;
                    entity.CopyProperties(loadedEntity);
                    loadedEntity.Id = savedId; // Load all except the Id

                    _entityService.Update(loadedEntity, user);
                    _dbContext.Update(loadedEntity);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericCodeRepository<TEntity, TDbContext>).FullName,
                    nameof(UpdateAsync),
                    JsonConvert.SerializeObject(entitiesList),
                    user);

                throw;
            }
        }

        public async Task DeleteAsync(string code, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    throw new ArgumentException(nameof(code), $"Cannot delete an entity of type {typeof(TEntity).Name} with a null or whitespace code.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot delete an entity of type {typeof(TEntity).Name} using a null user.");

                var entity = await GetByCodeAsync(code, new List<string>(), cancellationToken);
                if (entity == null)
                    return;

                _entityService.Delete(entity, user);
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericCodeRepository<TEntity, TDbContext>).FullName,
                    nameof(DeleteAsync),
                    code,
                    user);

                throw;
            }
        }

        public async Task DeleteAsync(List<string> codesList, IdentityUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (codesList == null || codesList.Count == 0)
                    throw new ArgumentException(nameof(codesList), $"Cannot delete an entity list of type {typeof(TEntity).Name} with a null or empty code list.");
                if (user == null)
                    throw new ArgumentNullException(nameof(user), $"Cannot delete an entity of type {typeof(TEntity).Name} using a null user.");

                var entitiesList = await GetByCodeListAsync(codesList, new List<string>(), cancellationToken);
                if (entitiesList == null)
                    return;

                foreach (var entity in entitiesList)
                {
                    _entityService.Delete(entity, user);
                    _dbContext.Update(entity);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.ErrorEx(ex,
                    typeof(GenericCodeRepository<TEntity, TDbContext>).FullName,
                    nameof(DeleteAsync),
                    JsonConvert.SerializeObject(codesList),
                    user);

                throw;
            }
        }
    }
}