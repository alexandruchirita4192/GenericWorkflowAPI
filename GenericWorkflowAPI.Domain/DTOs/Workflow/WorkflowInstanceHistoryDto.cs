using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowInstanceHistoryDto : IWorkflowInstanceDto
    {
        [Required]
        public string InstanceCode { get; set; }

        public string CurrentStateCode { get; set; }

        [Required]
        public string NextStateCode { get; set; }
    }
}