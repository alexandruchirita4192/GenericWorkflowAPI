using System.ComponentModel.DataAnnotations;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class IdentityUserDto : IBaseDto
    {
        [Required]
        public string UserName { get; set; }
    }
}