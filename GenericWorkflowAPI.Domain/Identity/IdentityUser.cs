using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GenericWorkflowAPI.Domain
{
    /// <summary>
    /// Identity user
    /// </summary>
    /// <remarks>No need to create a dto because it's not received or returned by the API.</remarks>
    public class IdentityUser : IdentityUser<long>, IBaseEntity, IBaseDto
    {
        #region Constructors

        public IdentityUser()
        {
        }

        public IdentityUser(string userName)
            : base(userName)
        {
        }

        #endregion Constructors

        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ChangedDate { get; set; }

        public bool IsDeleted { get { return false; } set { } }

        public long? ChangedByUserId { get; set; }

        [ForeignKey("ChangedByUserId")]
        public IdentityUser ChangedByUser { get; set; }

        public ICollection<IdentityUserClaim<string>> Claims { get; set; }
    }
}