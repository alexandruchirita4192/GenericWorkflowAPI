using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowInstanceInputCodeDto : IWorkflowInstanceDto
    {
        [Required]
        public string InstanceCode { get; set; }

        [Required]
        public string TypeCode { get; set; }

        [Required]
        public string Value { get; set; }
    }
}