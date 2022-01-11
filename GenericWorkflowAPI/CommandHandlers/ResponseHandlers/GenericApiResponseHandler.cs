using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GenericWorkflowAPI.CommandHandlers
{
    public class GenericApiResponseHandler<TPayload> : IRequestHandler<GenericApiResponse<TPayload>, ActionResult>
        where TPayload : class
    {
        private readonly ILogger logger;
        private readonly IHttpContextAccessor accessor;

        public GenericApiResponseHandler(ILogger _logger, IHttpContextAccessor _accessor)
        {
            logger = _logger;
            accessor = _accessor;
        }

        /// <summary>
        /// Handle all GenericApiResponse<TPayload> (transform a generic API response to ActionResult providing TPayload or error messages as occured)
        /// </summary>
        public async Task<ActionResult> Handle(GenericApiResponse<TPayload> response, CancellationToken cancellationToken)
        {
            if (response == null)
            {
                logger.Error($"{nameof(GenericApiResponseHandler<TPayload>)}.{nameof(Handle)} received empty response");
                return Problem((int)HttpStatusCode.InternalServerError, ValidationConstants.GenericValidationMessage, response.Extensions);
            }

            // 2xx - Successful status codes
            if (response.Status == HttpStatusCode.OK)
            {
                if (response.Payload != null)
                    return Ok(response.Payload);
                if (!string.IsNullOrWhiteSpace(response.Message))
                    return Ok(response.Message);
                return Ok();
            }
            if (response.Status == HttpStatusCode.Created)
                return Created(response.Payload);
            if (response.Status == HttpStatusCode.NoContent)
                return NoContent();

            if (response.Status == null)
                logger.Debug($"Unexpected null status in response {typeof(GenericApiResponse<TPayload>).FullName}; defaulting status to InternalServerError");

            var status = response.Status ?? HttpStatusCode.InternalServerError;
            var statusInt = (int)status;

            // 1xx – Informational, 2xx – Successful, 3xx – Redirection
            if (statusInt >= 100 && statusInt <= 399)
                return StatusCode(statusInt);

            if (string.IsNullOrWhiteSpace(response.Message))
                return Problem(statusInt);

            return Problem(statusInt, response.Message, response.Extensions);
        }

        #region Internal methods

        private ActionResult Ok()
        {
            return new OkResult();
        }

        private ActionResult Ok(string message)
        {
            return new OkObjectResult(message);
        }

        private ActionResult Ok(TPayload payload)
        {
            return new OkObjectResult(payload);
        }

        private ActionResult Created(TPayload payload)
        {
            return new CreatedResult(accessor?.HttpContext?.Request?.Path, payload);
        }

        private ActionResult NoContent()
        {
            return new NoContentResult();
        }

        private ActionResult StatusCode(int statusInt)
        {
            return new StatusCodeResult(statusInt);
        }

        private ActionResult Problem(int statusCode)
        {
            return Problem(statusCode, string.Empty, new Dictionary<string, object>());
        }

        private ActionResult Problem(int statusCode, string genericValidationMessage, Dictionary<string, object> invalidParams)
        {
            var problemDetails = GetProblemDetails(statusCode, genericValidationMessage, invalidParams);
            if (problemDetails == null)
                return new StatusCodeResult(statusCode);
            return new ObjectResult(problemDetails) { StatusCode = statusCode };
        }

        private ProblemDetails GetProblemDetails(int statusCode, string detail, Dictionary<string, object> invalidParams)
        {
            var hasInvalidParams = invalidParams != null && invalidParams.Count > 0;

            var problemDetails = new ProblemDetails
            {
                Type = $"{accessor?.HttpContext?.Request?.Path ?? "http://example.com"}"
                    + (hasInvalidParams ? "/Invalid-Parameters" : "/Error"),
                Title = hasInvalidParams ? ValidationConstants.InvalidRequestValidationTitle : ValidationConstants.GenericValidationTitle,
                Instance = accessor?.HttpContext?.Request?.Path,
                Detail = !string.IsNullOrWhiteSpace(detail) ? detail : ValidationConstants.GenericValidationMessage,
                Status = statusCode
            };

            // Add invalid parameters
            if (hasInvalidParams)
                foreach (var param in invalidParams)
                    problemDetails.Extensions[param.Key] = param.Value;

            return problemDetails;
        }

        #endregion Internal methods
    }
}