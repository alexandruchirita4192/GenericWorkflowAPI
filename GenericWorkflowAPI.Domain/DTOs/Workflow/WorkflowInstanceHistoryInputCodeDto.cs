using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Expand]
    [Page(MaxTop = 5, PageSize = 1)]
    [Select(SelectType = SelectExpandType.Automatic)]
    public class WorkflowInstanceHistoryInputCodeDto : IBaseDto
    {
        [Filter]
        [OrderBy]
        [Required]
        public string HistoryCode { get; set; }

        [JsonIgnore]
        public WorkflowInstanceHistoryDto History { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string InputCodeTypeCode { get; set; }

        [JsonIgnore]
        public WorkflowInputCodeTypeDto InputCodeType { get; set; }

        [Filter]
        public string Value { get; set; }
    }
}