using System.ComponentModel.DataAnnotations;
using GenericWorkflowAPI.Domain.Entities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// The type of workflow used for categorizing workflows by their type.
    /// </summary>
    [Index(nameof(Code), IsUnique = true)]
    public class WorkflowType : BaseEntity, IDescriptibleEntity
    {
        #region Constructors

        public WorkflowType()
        {
        }

        public WorkflowType(long? ticks, string? suffix = null)
        {
            this.FillEntity(ticks, suffix);
        }

        #endregion Constructors

        [Required]
        [StringLength(100)]
        public string? Code { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }
}