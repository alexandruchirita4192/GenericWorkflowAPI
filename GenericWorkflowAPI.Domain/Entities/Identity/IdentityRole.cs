using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GenericWorkflowAPI.Domain.Entities
{
    public class IdentityRole : IdentityRole<long>, ICodeEntity
    {
        #region Constructors

        public IdentityRole()
        {
        }

        public IdentityRole(string roleName)
            : base(roleName)
        {
        }

        #endregion Constructors

        public string Code { get { return Name; } set { Name = Code; } }

        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ChangedDate { get; set; }

        public bool IsDeleted { get { return false; } set { } }

        public long? ChangedByUserId { get; set; }

        [ForeignKey("ChangedByUserId")]
        public IdentityUser ChangedByUser { get; set; }
    }
}