using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Expand]
    [Page(MaxTop = 5, PageSize = 1)]
    [Select(SelectType = SelectExpandType.Automatic)]
    public class WorkflowTypeDto : IDescriptibleDto
    {
        [Filter]
        [OrderBy]
        [Required]
        public string Code { get; set; }

        [Filter]
        [OrderBy]
        public string Name { get; set; }

        [Filter]
        public string Description { get; set; }
    }
}