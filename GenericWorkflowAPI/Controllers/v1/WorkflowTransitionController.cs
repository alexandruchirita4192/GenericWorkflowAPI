using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Serilog;

namespace GenericWorkflowAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class WorkflowTransitionController : GenericCRUDController<WorkflowTransition, WorkflowTransitionDto>
    {
        public WorkflowTransitionController(ILogger loggerManager, IMediator mediator)
            : base(loggerManager, mediator,
                  new List<string> {
                      $"{nameof(WorkflowTransition.Workflow)}",
                      $"{nameof(WorkflowTransition.CurrentState)}",
                      $"{nameof(WorkflowTransition.NextState)}",
                      $"{nameof(WorkflowTransition.ChangedByUser)}"
                  })
        {
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

        [ValidateAntiForgeryToken]
        [HttpPost("{code}")]
        public async Task<IActionResult> Create([FromBody] WorkflowTransitionDto item, CancellationToken cancellationToken)
        {
            return await base.CreateItem(item, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Collection<WorkflowTransitionDto> collection, CancellationToken cancellationToken)
        {
            return await base.CreateCollection(collection, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPut("{code}")]
        public async Task<IActionResult> Update([FromBody] WorkflowTransitionDto item, CancellationToken cancellationToken)
        {
            return await base.UpdateItem(item, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Collection<WorkflowTransitionDto> collection, CancellationToken cancellationToken)
        {
            return await base.UpdateCollection(collection, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code, CancellationToken cancellationToken)
        {
            return await base.DeleteItem(code, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Collection<string> codes, CancellationToken cancellationToken)
        {
            return await base.DeleteCollection(codes, cancellationToken);
        }
    }
}