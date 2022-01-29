using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// A record of an executed transition of the workflow instance (<see cref="Instance"/>) defined by the states:
    /// the current state (<see cref="CurrentState"/>) and the next state (<see cref="NextState"/>).
    /// </summary>
    public class WorkflowInstanceHistory : BaseEntity, IWorkflowInstanceEntity
    {
        [Required]
        public long? InstanceId { get; set; }

        [ForeignKey("InstanceId")] // $"{nameof(InstanceId)}"
        public WorkflowInstance? Instance { get; set; }

        [Required]
        public long? CurrentStateId { get; set; }

        [ForeignKey("CurrentStateId")] // $"{nameof(CurrentStateId)}"
        public WorkflowState? CurrentState { get; set; }

        [Required]
        public long? NextStateId { get; set; }

        [ForeignKey("NextStateId")] // $"{nameof(NextStateId)}"
        public WorkflowState? NextState { get; set; }
    }
}