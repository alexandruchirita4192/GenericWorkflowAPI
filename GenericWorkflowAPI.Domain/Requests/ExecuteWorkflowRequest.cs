using System.Collections.Generic;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class ExecuteWorkflowRequest : IRequest<GenericApiResponse<string>>
    {
        public string WorkflowCode { get; set; }
        public string WorkflowInstanceCode { get; set; }
        public Dictionary<string, string> WorkflowInputCodeTypeXvalue { get; set; }
    }
}