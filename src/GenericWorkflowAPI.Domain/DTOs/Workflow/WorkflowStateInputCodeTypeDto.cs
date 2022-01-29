using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(Code), nameof(StateCode), nameof(InputCodeTypeCode))]
    public class WorkflowStateInputCodeTypeDto : IWorkflowStateDto, ICodeDto
    {
        [Required]
        public string? Code { get; set; }

        [Required]
        public string? StateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto? State { get; set; }

        [Required]
        public string? InputCodeTypeCode { get; set; }

        [JsonIgnore]
        public WorkflowInputCodeTypeDto? InputCodeType { get; set; }
    }
}