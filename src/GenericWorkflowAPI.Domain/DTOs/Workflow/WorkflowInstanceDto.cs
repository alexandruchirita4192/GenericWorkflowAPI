using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(WorkflowCode), nameof(Code), nameof(CurrentStateCode))]
    public class WorkflowInstanceDto : ICodeDto, IWorkflowDto
    {
        [Required]
        public string? WorkflowCode { get; set; }

        [JsonIgnore]
        public WorkflowDto? Workflow { get; set; }

        [Required]
        public string? Code { get; set; }

        [Required]
        public string? CurrentStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto? CurrentState { get; set; }
    }
}