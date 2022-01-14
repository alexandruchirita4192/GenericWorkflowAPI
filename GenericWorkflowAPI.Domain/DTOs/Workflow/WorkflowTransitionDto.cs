using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(Code), nameof(WorkflowCode), nameof(CurrentStateCode), nameof(NextStateCode), nameof(RoleCode))]
    public class WorkflowTransitionDto : IWorkflowDto, ICodeDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string WorkflowCode { get; set; }

        [JsonIgnore]
        public WorkflowDto Workflow { get; set; }

        [Required]
        public string CurrentStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto CurrentState { get; set; }

        [Required]
        public string NextStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto NextState { get; set; }

        [Required]
        public string RoleCode { get; set; }
    }
}