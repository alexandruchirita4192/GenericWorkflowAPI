using System;

namespace GenericWorkflowAPI.Domain.Entities
{
    public interface IBaseEntity : IIdEntity
    {
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ChangedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long? ChangedByUserId { get; set; }
        public IdentityUser? ChangedByUser { get; set; }
    }
}