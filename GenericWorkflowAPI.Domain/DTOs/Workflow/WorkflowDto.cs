using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowDto : IDescriptibleDto
    {
        [Required]
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public string TypeCode { get; set; }
    }
}