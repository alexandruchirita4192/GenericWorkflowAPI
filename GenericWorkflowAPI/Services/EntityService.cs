using System;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.Entities;

namespace GenericWorkflowAPI.Services
{
    /// <summary>
    /// Service to update entities properties
    /// </summary>
    /// <remarks>It doesn't update the database in any way.</remarks>
    public class EntityService<TEntity> : IEntityService<TEntity>
        where TEntity : class, IBaseEntity, new()
    {
        public void Initialize(TEntity entity)
        {
            if (entity == null)
                throw new InvalidOperationException($"Cannot initialize a null entity of type {typeof(TEntity).Name}!");

            entity.CreatedDate = DateTimeOffset.Now;
            Update(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new InvalidOperationException($"Cannot update a null entity of type {typeof(TEntity).Name}!");

            entity.ChangedDate = DateTimeOffset.Now;
            entity.ChangedByUserId = 1; // TODO: Set the current logged in user as entity.ChangedByUserId
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new InvalidOperationException($"Cannot soft delete a null entity of type {typeof(TEntity).Name}!");

            entity.IsDeleted = true;
            Update(entity);
        }
    }
}