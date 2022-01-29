using System.Collections.Generic;

namespace GenericWorkflowAPI.Domain.DTOs
{
    public class ExecuteWorkflowRequestDto : IBaseDto
    {
        public string? WorkflowCode { get; set; }
        public string? WorkflowInstanceCode { get; set; }
        public Dictionary<string, string>? WorkflowInputCodeTypeXvalue { get; set; }
    }
}