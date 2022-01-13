using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class ExecuteWorkflowRequest : IRequest<GenericApiResponse<string>>
    {
        [NotMapped]
        public IdentityUser User { get; set; }
        public string WorkflowCode { get; set; }
        public string WorkflowInstanceCode { get; set; }
        public Dictionary<string, string> WorkflowInputCodeTypeXvalue { get; set; }
    }
}