using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericWorkflowAPI.Domain.Entities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// A state of the workflow (<see cref="Workflow"/>) is a step in the workflow.
    /// Transitions (<see cref="WorkflowTransition"/>) define where the workflow can go (from one state to another).
    /// </summary>
    [Index(nameof(Code), IsUnique = true)]
    public class WorkflowState : BaseEntity, IDescriptibleEntity, IWorkflowEntity
    {
        #region Constructors

        public WorkflowState()
        {
        }

        public WorkflowState(long? ticks, string? suffix = null)
        {
            this.FillEntity(ticks, suffix);
        }

        #endregion Constructors

        [Required]
        public long? WorkflowId { get; set; }

        [ForeignKey("WorkflowId")] // $"{nameof(WorkflowId)}"
        public Workflow? Workflow { get; set; }

        [Required]
        [StringLength(100)]
        public string? Code { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public bool? IsFirstState { get; set; }
    }
}