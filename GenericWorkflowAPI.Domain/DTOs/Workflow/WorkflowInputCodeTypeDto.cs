using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Expand]
    [Page(MaxTop = 5, PageSize = 1)]
    [Select(SelectType = SelectExpandType.Automatic)]
    public class WorkflowInputCodeTypeDto : IDescriptibleDto, IWorkflowDto
    {
        [Filter]
        [OrderBy]
        [Required]
        public string WorkflowCode { get; set; }

        [JsonIgnore]
        public WorkflowDto Workflow { get; set; }

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