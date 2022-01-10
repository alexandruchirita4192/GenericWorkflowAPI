using GenericWorkflowAPI.Domain.Entities;

namespace GenericWorkflowAPI.Core.Services
{
    public interface IEntityService<TEntity> where TEntity : class, IBaseEntity, new()
    {
        void Initialize(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}