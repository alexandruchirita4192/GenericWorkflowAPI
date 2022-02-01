using System.ComponentModel.DataAnnotations;
using GenericWorkflowAPI.Domain.DTOs.Extensions;
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
        #region Constructors

        public WorkflowStateInputCodeTypeDto()
        {
        }

        public WorkflowStateInputCodeTypeDto(long? ticks, string? suffix = null)
        {
            this.FillDto(ticks, suffix);
        }

        #endregion Constructors

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