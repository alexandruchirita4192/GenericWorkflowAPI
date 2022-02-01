using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericWorkflowAPI.Domain.Entities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// An input code type associated to the workflow (<see cref="Workflow"/>).
    /// </summary>
    [Index(nameof(Code), IsUnique = true)]
    public class WorkflowInputCodeType : BaseEntity, IDescriptibleEntity, IWorkflowEntity
    {
        #region Constructors

        public WorkflowInputCodeType()
        {
        }

        public WorkflowInputCodeType(long? ticks, string? suffix = null)
        {
            this.FillEntity(ticks, suffix);
        }

        #endregion Constructors

        [Required]
        public long? WorkflowId { get; set; }

        [ForeignKey("WorkflowId")] //$"{nameof(WorkflowId)}"
        public Workflow? Workflow { get; set; }

        [Required]
        [StringLength(100)]
        public string? Code { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // TODO: Extend later with DataType of input code, RegexPattern for validation
    }
}