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
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(GetByCodeAsync)}({code}) function error");
            }
            return default(TEntity);
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
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(GetByCodeListAsync)}({JsonConvert.SerializeObject(codesList)}) function error");
            }
            return default(List<TEntity>);
        }

        public async Task DeleteAsync(string code, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(code))
                return;

            try
            {
                var entity = await GetByCodeAsync(code, new List<string>(), cancellationToken);
                if (entity == null)
                    return;

                _entityService.Delete(entity);
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(DeleteAsync)}({code}) function error");
            }
        }

        public async Task DeleteAsync(List<string> codesList, CancellationToken cancellationToken)
        {
            if (codesList == null || codesList.Count == 0)
                return;

            try
            {
                var entitiesList = await GetByCodeListAsync(codesList, new List<string>(), cancellationToken);
                if (entitiesList == null)
                    return;

                foreach (var entity in entitiesList)
                {
                    _entityService.Delete(entity);
                    _dbContext.Update(entity);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericRepository<TEntity, TDbContext>)}.{nameof(DeleteAsync)}({JsonConvert.SerializeObject(codesList)}) function error");
            }
        }
    }
}