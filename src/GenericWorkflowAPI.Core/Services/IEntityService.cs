using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Entities;

namespace GenericWorkflowAPI.Core.Services
{
    public interface IEntityService<TEntity> where TEntity : class, IBaseEntity, new()
    {
        void Initialize(TEntity entity, IdentityUser user);

        void Update(TEntity entity, IdentityUser user);

        void Delete(TEntity entity, IdentityUser user);
    }
}