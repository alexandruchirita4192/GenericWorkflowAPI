using System;
using System.ComponentModel.DataAnnotations.Schema;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GenericWorkflowAPI.Domain
{
    /// <summary>
    /// Identity roles
    /// </summary>
    /// <remarks>No need to create a dto because it's not received or returned by the API.</remarks>
    public class IdentityRole : IdentityRole<long>, ICodeEntity, ICodeDto
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

        public string? Code { get { return Name; } set { Name = Code; } }

        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ChangedDate { get; set; }

        public bool IsDeleted { get { return false; } set { } }

        public long? ChangedByUserId { get; set; }

        [ForeignKey("ChangedByUserId")]
        public IdentityUser? ChangedByUser { get; set; }
    }
}