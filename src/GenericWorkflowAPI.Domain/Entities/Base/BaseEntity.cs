using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericWorkflowAPI.Domain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        public long Id { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ChangedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long? ChangedByUserId { get; set; }

        [ForeignKey("ChangedByUserId")]
        public IdentityUser? ChangedByUser { get; set; }
    }
}