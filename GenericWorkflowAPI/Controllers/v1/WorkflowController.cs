using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Attributes;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Domain.Responses;
using GenericWorkflowAPI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Serilog;

namespace GenericWorkflowAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class WorkflowController : GenericCRUDController<Workflow, WorkflowDto>
    {
        public WorkflowController(ILogger loggerManager, IMediator mediator)
            : base(loggerManager, mediator,
                  new List<string> {
                      $"{nameof(Workflow.Type)}",
                      $"{nameof(Workflow.ChangedByUser)}"
                  })
        {
        }

        /// <summary>
        /// Initialize workflow instance or advance workflow instance state based on defined transitions and required input codes
        /// </summary>
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("ExecuteWorkflow")]
        public async Task<IActionResult> ExecuteWorkflow([FromBody] ExecuteWorkflowRequestDto executeWorkflowDto, CancellationToken cancellationToken)
        {
            ExecuteWorkflowRequest executeWorkflowRequest;

            // Fill execute workflow request from Dto and context (user)
            try
            {
                executeWorkflowRequest = new ExecuteWorkflowRequest()
                {
                    User = this.GetUser(),
                    WorkflowCode = executeWorkflowDto.WorkflowCode,
                    WorkflowInstanceCode = executeWorkflowDto.WorkflowInstanceCode,
                    WorkflowInputCodeTypeXvalue = executeWorkflowDto.WorkflowInputCodeTypeXvalue
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error received while setting the user to an {requestName}.", nameof(ExecuteWorkflowRequest));

                var errorApiResponse = GenericApiResponse<string>.Problem(
                    ValidationConstants.GenericValidationMessage,
                    HttpStatusCode.InternalServerError);
                var errorResponse = await _mediator.Send(errorApiResponse, cancellationToken);
                return errorResponse;
            }

            var genericApiResponse = await _mediator.Send(executeWorkflowRequest, cancellationToken);
            var response = await _mediator.Send(genericApiResponse, cancellationToken);
            return response;
        }

        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string code, CancellationToken cancellationToken)
        {
            return await base.GetItem(code, cancellationToken);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return await base.GetCollection(cancellationToken);
        }

        [EnableQuery]
        [HttpGet("Queryable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IQueryable<WorkflowDto>> Get([OpenApiParameterIgnore] ODataQueryOptions<WorkflowDto> queryOptions, CancellationToken cancellationToken)
        {
            return await base.GetQueryable(queryOptions, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{code}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] WorkflowDto item, CancellationToken cancellationToken)
        {
            return await base.CreateItem(item, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] Collection<WorkflowDto> collection, CancellationToken cancellationToken)
        {
            return await base.CreateCollection(collection, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] WorkflowDto item, CancellationToken cancellationToken)
        {
            return await base.UpdateItem(item, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Collection<WorkflowDto> collection, CancellationToken cancellationToken)
        {
            return await base.UpdateCollection(collection, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string code, CancellationToken cancellationToken)
        {
            return await base.DeleteItem(code, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] Collection<string> codes, CancellationToken cancellationToken)
        {
            return await base.DeleteCollection(codes, cancellationToken);
        }
    }
}