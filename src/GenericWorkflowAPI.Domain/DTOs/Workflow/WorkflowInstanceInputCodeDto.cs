using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(InstanceCode), nameof(TypeCode))]
    public class WorkflowInstanceInputCodeDto : IWorkflowInstanceDto
    {
        [Required]
        public string? InstanceCode { get; set; }

        [JsonIgnore]
        public WorkflowInstanceDto? Instance { get; set; }

        [Required]
        public string? TypeCode { get; set; }

        [JsonIgnore]
        public WorkflowInputCodeTypeDto? Type { get; set; }

        [Required]
        public string? Value { get; set; }
    }
}