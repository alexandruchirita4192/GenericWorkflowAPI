using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //[ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("ExecuteWorkflow")]
        public async Task<IActionResult> ExecuteWorkflow([FromBody] ExecuteWorkflowRequest executeWorkflowRequest, CancellationToken cancellationToken)
        {
            var genericApiResponse = await _mediator.Send(executeWorkflowRequest, cancellationToken);
            var response = await _mediator.Send(genericApiResponse, cancellationToken);
            return response;
        }

        //[EnableQuery]
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code, CancellationToken cancellationToken)
        {
            return await base.GetItem(code, cancellationToken);
        }

        //[EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return await base.GetCollection(cancellationToken);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("{code}")]
        public async Task<IActionResult> Create([FromBody] WorkflowDto item, CancellationToken cancellationToken)
        {
            return await base.CreateItem(item, cancellationToken);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Collection<WorkflowDto> collection, CancellationToken cancellationToken)
        {
            return await base.CreateCollection(collection, cancellationToken);
        }

        //[ValidateAntiForgeryToken]
        [HttpPut("{code}")]
        public async Task<IActionResult> Update([FromBody] WorkflowDto item, CancellationToken cancellationToken)
        {
            return await base.UpdateItem(item, cancellationToken);
        }

        //[ValidateAntiForgeryToken]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Collection<WorkflowDto> collection, CancellationToken cancellationToken)
        {
            return await base.UpdateCollection(collection, cancellationToken);
        }

        //[ValidateAntiForgeryToken]
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code, CancellationToken cancellationToken)
        {
            return await base.DeleteItem(code, cancellationToken);
        }

        //[ValidateAntiForgeryToken]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Collection<string> codes, CancellationToken cancellationToken)
        {
            return await base.DeleteCollection(codes, cancellationToken);
        }
    }
}