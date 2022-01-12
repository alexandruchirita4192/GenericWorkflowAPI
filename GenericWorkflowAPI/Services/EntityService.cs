using System;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain;
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
        public void Initialize(TEntity entity, IdentityUser user)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"Cannot initialize a null entity of type {typeof(TEntity).Name}!");
            if (user == null)
                throw new ArgumentNullException(nameof(user), $"Cannot initialize an entity using a null user!");

            entity.CreatedDate = DateTimeOffset.Now;
            Update(entity, user);
        }

        public void Update(TEntity entity, IdentityUser user)
        {
            if (entity == null)
                throw new ArgumentNullException($"Cannot update a null entity of type {typeof(TEntity).Name}!");
            if (user == null)
                throw new ArgumentNullException(nameof(user), $"Cannot update an entity using a null user!");

            entity.ChangedDate = DateTimeOffset.Now;
            entity.ChangedByUserId = user.Id;
        }

        public void Delete(TEntity entity, IdentityUser user)
        {
            if (entity == null)
                throw new ArgumentNullException($"Cannot soft delete a null entity of type {typeof(TEntity).Name}!");
            if (user == null)
                throw new ArgumentNullException(nameof(user), $"Cannot soft delete an entity using a null user!");

            entity.IsDeleted = true;
            Update(entity, user);
        }
    }
}