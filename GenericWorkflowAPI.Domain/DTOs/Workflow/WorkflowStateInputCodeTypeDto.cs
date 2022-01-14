using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Expand]
    [Page(MaxTop = 5, PageSize = 1)]
    [Select(SelectType = SelectExpandType.Automatic)]
    public class WorkflowStateInputCodeTypeDto : IWorkflowStateDto, ICodeDto
    {
        [Filter]
        [OrderBy]
        [Required]
        public string Code { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string StateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto State { get; set; }

        [Filter]
        [OrderBy]
        [Required]
        public string InputCodeTypeCode { get; set; }

        [JsonIgnore]
        public WorkflowInputCodeTypeDto InputCodeType { get; set; }
    }
}