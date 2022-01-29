using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Services;
using GenericWorkflowAPI.Domain.Constants;
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
                if (cancellationToken.IsCancellationRequested)
                    return GenericApiResponse<string>.Ok();
                if (request == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request)), $"Invalid request of type {typeof(ExecuteWorkflowRequest).FullName}");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict);
                }
                if (request.User == null)
                {
                    _logger.Error(new ArgumentNullException(nameof(request.User)), $"Cannot handle request of type {typeof(ExecuteWorkflowRequest).FullName} for null user.");
                    return GenericApiResponse<string>.Problem(ValidationConstants.InvalidRequestValidationTitle, HttpStatusCode.Conflict,
                        new Dictionary<string, object> { { $"{nameof(request.User)}", ValidationConstants.InvalidUserMessage } });
                }

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