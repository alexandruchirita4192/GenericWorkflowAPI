using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericWorkflowAPI.Domain.Entities
{
    /// <summary>
    /// A record of an aquired input code of type <see cref="InputCodeType"/> associated at a time with an workflow instance (<see cref="Instance"/>)
    /// with value (<see cref="Value"/>).
    /// </summary>
    public class WorkflowInstanceHistoryInputCode : BaseEntity
    {
        [Required]
        public long? HistoryId { get; set; }

        [ForeignKey("HistoryId")] // $"{nameof(HistoryId)}"
        public WorkflowInstanceHistory History { get; set; }

        [Required]
        public long? InputCodeTypeId { get; set; }

        [ForeignKey("InputCodeTypeId")] // $"{nameof(InputCodeTypeId)}"
        public WorkflowInputCodeType InputCodeType { get; set; }

        [StringLength(1000)]
        public string Value { get; set; }
    }
}