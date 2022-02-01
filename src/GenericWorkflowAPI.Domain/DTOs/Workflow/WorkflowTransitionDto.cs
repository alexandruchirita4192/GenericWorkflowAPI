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
    [OrderBy(nameof(Code), nameof(WorkflowCode), nameof(CurrentStateCode), nameof(NextStateCode), nameof(RoleCode))]
    public class WorkflowTransitionDto : IWorkflowDto, ICodeDto
    {
        #region Constructors

        public WorkflowTransitionDto()
        {
        }

        public WorkflowTransitionDto(long? ticks, string? suffix = null)
        {
            this.FillDto(ticks, suffix);
        }

        #endregion Constructors

        [Required]
        public string? Code { get; set; }

        [Required]
        public string? WorkflowCode { get; set; }

        [JsonIgnore]
        public WorkflowDto? Workflow { get; set; }

        [Required]
        public string? CurrentStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto? CurrentState { get; set; }

        [Required]
        public string? NextStateCode { get; set; }

        [JsonIgnore]
        public WorkflowStateDto? NextState { get; set; }

        [Required]
        public string? RoleCode { get; set; }
    }
}