using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.Entities;

namespace GenericWorkflowAPI.Core.Services
{
    public interface IGenericCodeRepository<TEntity> : IGenericRepository<TEntity>, IGenericCodeRepository
        where TEntity : class, IBaseEntity, ICodeEntity, new()
    {
        public Task<TEntity> GetByCodeAsync(string code, List<string> includePathList, CancellationToken cancellationToken);

        public Task<List<TEntity>> GetByCodeListAsync(List<string> codesList, List<string> includePathList, CancellationToken cancellationToken);

        public Task DeleteAsync(string code, CancellationToken cancellationToken);

        public Task DeleteAsync(List<string> codesList, CancellationToken cancellationToken);
    }

    public interface IGenericCodeRepository
    {
        public Task<ICodeEntity> GetInterfaceTypeByCodeAsync(string code, List<string> includePathList, CancellationToken cancellationToken);
    }
}