using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.Requests;

namespace GenericWorkflowAPI.Core.Services
{
    public interface IWorkflowService
    {
        Task Run(ExecuteWorkflowRequest executeWorkflowRequest, CancellationToken cancellationToken);
    }
}