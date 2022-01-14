using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Expand]
    [Page(MaxTop = 5, PageSize = 1)]
    [Select(SelectType = SelectExpandType.Automatic)]
    public class WorkflowInstanceHistoryDto : IWorkflowInstanceDto
    {
        [Filter]
        [OrderBy]
        [Required]
        public string InstanceCode { get; set; }

        [JsonIgnore]
        public WorkflowInstanceDto Instance { get; set; }

        [Filter]
        [OrderBy]
        public string CurrentStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto CurrentState { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string NextStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto NextState { get; set; }
    }
}