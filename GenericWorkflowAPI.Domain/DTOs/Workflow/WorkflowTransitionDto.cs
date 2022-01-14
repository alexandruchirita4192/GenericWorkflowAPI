using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Expand]
    [Page(MaxTop = 5, PageSize = 1)]
    [Select(SelectType = SelectExpandType.Automatic)]
    public class WorkflowTransitionDto : IWorkflowDto, ICodeDto
    {
        [Filter]
        [OrderBy]
        [Required]
        public string Code { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string WorkflowCode { get; set; }

        [JsonIgnore]
        public WorkflowDto Workflow { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string CurrentStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto CurrentState { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string NextStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto NextState { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string RoleCode { get; set; }
    }
}