using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowStateDto : IDescriptibleDto, IWorkflowDto
    {
        [Required]
        public string WorkflowCode { get; set; }

        [Required]
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool? IsFirstState { get; set; }
    }
}