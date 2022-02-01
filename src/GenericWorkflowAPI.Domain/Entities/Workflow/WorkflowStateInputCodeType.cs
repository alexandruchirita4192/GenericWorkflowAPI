using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericWorkflowAPI.Domain.Entities.Extensions;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// An association between a workflow state (<see cref="State"/>) and a workflow input code type (<see cref="InputCodeType"/>),
    /// defining which workflow input code types are required by each state.
    /// </summary>
    public class WorkflowStateInputCodeType : BaseEntity, IWorkflowStateEntity, ICodeEntity
    {
        #region Constructors

        public WorkflowStateInputCodeType()
        {
        }

        public WorkflowStateInputCodeType(long? ticks, string? suffix = null)
        {
            this.FillEntity(ticks, suffix);
        }

        #endregion Constructors

        [Required]
        [StringLength(100)]
        public string? Code { get; set; }

        [Required]
        public long? StateId { get; set; }

        [ForeignKey("StateId")] // $"{nameof(StateId)}"
        public WorkflowState? State { get; set; }

        [Required]
        public long? InputCodeTypeId { get; set; }

        [ForeignKey("InputCodeTypeId")] // $"{nameof(InputCodeTypeId)}"
        public WorkflowInputCodeType? InputCodeType { get; set; }
    }
}