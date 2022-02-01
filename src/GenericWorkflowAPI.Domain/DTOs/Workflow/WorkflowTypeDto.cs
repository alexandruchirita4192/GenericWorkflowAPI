using System.ComponentModel.DataAnnotations;
using GenericWorkflowAPI.Domain.DTOs.Extensions;
using Microsoft.OData.ModelBuilder;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Filter]
    [Expand]
    [Page(MaxTop = 5, PageSize = 10)]
    [Select(SelectType = SelectExpandType.Automatic)]
    [OrderBy(nameof(Code), nameof(Name))]
    public class WorkflowTypeDto : IDescriptibleDto
    {
        #region Constructors

        public WorkflowTypeDto()
        {
        }

        public WorkflowTypeDto(long? ticks, string? suffix = null)
        {
            this.FillDto(ticks, suffix);
        }

        #endregion Constructors

        [Required]
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}