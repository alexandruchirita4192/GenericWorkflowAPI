using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowInstanceDto : ICodeDto, IWorkflowDto
    {
        [Required]
        public string WorkflowCode { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string CurrentStateCode { get; set; }
    }
}