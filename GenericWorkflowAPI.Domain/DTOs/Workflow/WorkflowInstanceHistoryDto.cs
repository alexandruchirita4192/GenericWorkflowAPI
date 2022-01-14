using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(InstanceCode), nameof(CurrentStateCode), nameof(NextStateCode))]
    public class WorkflowInstanceHistoryDto : IWorkflowInstanceDto
    {
        [Required]
        public string InstanceCode { get; set; }

        [JsonIgnore]
        public WorkflowInstanceDto Instance { get; set; }

        public string CurrentStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto CurrentState { get; set; }

        [Required]
        public string NextStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto NextState { get; set; }
    }
}