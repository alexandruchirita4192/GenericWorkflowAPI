using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(HistoryCode), nameof(InputCodeTypeCode))]
    public class WorkflowInstanceHistoryInputCodeDto : IBaseDto
    {
        [Required]
        public string HistoryCode { get; set; }

        [JsonIgnore]
        public WorkflowInstanceHistoryDto History { get; set; }

        [Required]
        public string InputCodeTypeCode { get; set; }

        [JsonIgnore]
        public WorkflowInputCodeTypeDto InputCodeType { get; set; }

        public string Value { get; set; }
    }
}