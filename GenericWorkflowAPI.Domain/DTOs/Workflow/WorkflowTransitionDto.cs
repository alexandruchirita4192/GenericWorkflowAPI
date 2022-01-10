using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowTransitionDto : IWorkflowDto, ICodeDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string WorkflowCode { get; set; }

        [Required]
        public string CurrentStateCode { get; set; }

        [Required]
        public string NextStateCode { get; set; }
    }
}