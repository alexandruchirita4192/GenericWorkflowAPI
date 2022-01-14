using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(Code), nameof(Name))]
    public class WorkflowTypeDto : IDescriptibleDto
    {
        [Required]
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}