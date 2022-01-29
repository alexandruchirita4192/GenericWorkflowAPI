using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(Code), nameof(Name), nameof(TypeCode))]
    public class WorkflowDto : IDescriptibleDto
    {
        [Required]
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? TypeCode { get; set; }

        [JsonIgnore]
        public WorkflowTypeDto? Type { get; set; }
    }
}