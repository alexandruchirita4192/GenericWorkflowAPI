using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowInstanceHistoryInputCodeDto : IWorkflowInstanceDto
    {
        [Required]
        public string InstanceCode { get; set; }

        [Required]
        public string InputCodeTypeCode { get; set; }

        public string Value { get; set; }
    }
}