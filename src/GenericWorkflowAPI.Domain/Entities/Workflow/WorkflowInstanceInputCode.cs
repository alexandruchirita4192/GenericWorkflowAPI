using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// An input code associated to a workflow instance (<see cref="Instance"/>), having a workflow input code type (<see cref="Type"/>)
    /// and a value (<see cref="Value"/>).
    /// </summary>
    public class WorkflowInstanceInputCode : BaseEntity, IWorkflowInstanceEntity
    {
        [Required]
        public long? InstanceId { get; set; }

        [ForeignKey("InstanceId")] // $"{nameof(InstanceId)}"
        public WorkflowInstance? Instance { get; set; }

        [Required]
        public long? TypeId { get; set; }

        [ForeignKey("TypeId")] // $"{nameof(TypeId)}"
        public WorkflowInputCodeType? Type { get; set; }

        [Required]
        [StringLength(1000)]
        public string? Value { get; set; }
    }
}