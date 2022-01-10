using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers.RequestHandlers
{
    public class ExecuteWorkflowCommandHandler : IRequestHandler<ExecuteWorkflowRequest, GenericApiResponse<string>>
    {
        private readonly IWorkflowService _workflowService;
        private readonly ILogger _logger;

        public ExecuteWorkflowCommandHandler(IWorkflowService workflowService, ILogger logger)
        {
            _workflowService = workflowService;
            _logger = logger;
        }

        public async Task<GenericApiResponse<string>> Handle(ExecuteWorkflowRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _workflowService.Run(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Workflow execution failed with exception");
                return GenericApiResponse<string>.Problem(HttpStatusCode.InternalServerError);
            }
            return GenericApiResponse<string>.Ok();
        }
    }
}