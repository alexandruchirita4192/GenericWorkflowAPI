using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Expand]
    [Page(MaxTop = 5, PageSize = 1)]
    [Select(SelectType = SelectExpandType.Automatic)]
    public class WorkflowInstanceInputCodeDto : IWorkflowInstanceDto
    {
        [Filter]
        [OrderBy]
        [Required]
        public string InstanceCode { get; set; }

        [JsonIgnore]
        public WorkflowInstanceDto Instance { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string TypeCode { get; set; }

        [JsonIgnore]
        public WorkflowInputCodeTypeDto Type { get; set; }

        [Filter]
        [Required]
        public string Value { get; set; }
    }
}