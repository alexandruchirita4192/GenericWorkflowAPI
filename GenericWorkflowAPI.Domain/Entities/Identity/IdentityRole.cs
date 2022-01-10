using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GenericWorkflowAPI.Domain.Entities
{
    public class IdentityRole : IdentityRole<long>, IBaseEntity
    {
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ChangedDate { get; set; }

        [NotMapped]
        public bool IsDeleted { get { return false; } set { } }

        public long? ChangedByUserId { get; set; }

        [ForeignKey("ChangedByUserId")]
        public IdentityUser ChangedByUser { get; set; }
    }
}