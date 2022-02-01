using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericWorkflowAPI.Domain.Entities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// The workflow having a type (<see cref="Type"/>), a collection of states (<see cref="WorkflowState"/>),
    /// a collection of transitions (<see cref="WorkflowTransition"/>) and a collection of instances (<see cref="WorkflowInstance"/>).
    /// </summary>
    [Index(nameof(Code), IsUnique = true)]
    public class Workflow : BaseEntity, IDescriptibleEntity
    {
        #region Constructors

        public Workflow()
        {
        }

        public Workflow(long? ticks, string? suffix = null)
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

        public long? TypeId { get; set; }

        [ForeignKey("TypeId")] // $"{nameof(TypeId)}"
        public WorkflowType? Type { get; set; }
    }
}