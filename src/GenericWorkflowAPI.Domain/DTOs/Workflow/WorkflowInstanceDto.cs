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
    [OrderBy(nameof(WorkflowCode), nameof(Code), nameof(CurrentStateCode))]
    public class WorkflowInstanceDto : ICodeDto, IWorkflowDto
    {
        #region Constructors

        public WorkflowInstanceDto()
        {
        }

        public WorkflowInstanceDto(long? ticks, string? suffix = null)
        {
            this.FillDto(ticks, suffix);
        }

        #endregion Constructors

        [Required]
        public string? WorkflowCode { get; set; }

        [JsonIgnore]
        public WorkflowDto? Workflow { get; set; }

        [Required]
        public string? Code { get; set; }

        [Required]
        public string? CurrentStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto? CurrentState { get; set; }
    }
}