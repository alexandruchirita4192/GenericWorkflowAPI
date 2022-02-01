using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericWorkflowAPI.Domain.Entities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// An instance associated to the workflow (<see cref="Workflow"/>) having a state (<see cref="CurrentState"/>)
    /// and a collection of input codes associated to the instance (<see cref="WorkflowInstanceInputCode"/>).
    /// </summary>
    [Index(nameof(Code), IsUnique = true)]
    public class WorkflowInstance : BaseEntity, ICodeEntity, IWorkflowEntity
    {
        #region Constructors

        public WorkflowInstance()
        {
        }

        public WorkflowInstance(long? ticks, string? suffix = null)
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

        [Required]
        public long? CurrentStateId { get; set; }

        [ForeignKey("CurrentStateId")] // $"{nameof(CurrentStateId)}"
        public WorkflowState? CurrentState { get; set; }
    }
}