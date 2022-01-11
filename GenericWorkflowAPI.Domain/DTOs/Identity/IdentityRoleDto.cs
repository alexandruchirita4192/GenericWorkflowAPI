using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class IdentityRoleDto : ICodeDto
    {
        [Required]
        public string Code { get; set; }
    }
}