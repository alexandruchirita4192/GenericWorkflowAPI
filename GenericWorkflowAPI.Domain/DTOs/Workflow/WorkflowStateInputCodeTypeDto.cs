using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class WorkflowStateInputCodeTypeDto : IWorkflowStateDto, ICodeDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string StateCode { get; set; }

        [Required]
        public string InputCodeTypeCode { get; set; }
    }
}