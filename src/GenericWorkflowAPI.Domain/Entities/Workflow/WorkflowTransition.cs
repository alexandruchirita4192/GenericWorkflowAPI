using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// A transition of the <see cref="Workflow"/> defining where the workflow can go from the <see cref="CurrentState"/> to the <see cref="NextState"/>.
    /// </summary>
    public class WorkflowTransition : BaseEntity, IWorkflowEntity, ICodeEntity
    {
        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        public long? WorkflowId { get; set; }

        [ForeignKey("WorkflowId")] // $"{nameof(WorkflowId)}"
        public Workflow Workflow { get; set; }

        [Required]
        public long? CurrentStateId { get; set; }

        [ForeignKey("CurrentStateId")] // $"{nameof(CurrentStateId)}"
        public WorkflowState CurrentState { get; set; }

        [Required]
        public long? NextStateId { get; set; }

        [ForeignKey("NextStateId")] // $"{nameof(NextStateId)}"
        public WorkflowState NextState { get; set; }

        public long? RoleId { get; set; }

        [ForeignKey("RoleId")] // $"{nameof(RoleId)}"
        public IdentityRole Role { get; set; }
    }
}