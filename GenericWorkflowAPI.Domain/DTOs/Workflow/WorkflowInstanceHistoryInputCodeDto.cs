using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowInstanceHistoryInputCodeDto : IBaseDto
    {
        [Required]
        public string HistoryCode { get; set; }

        [Required]
        public string InputCodeTypeCode { get; set; }

        public string Value { get; set; }
    }
}