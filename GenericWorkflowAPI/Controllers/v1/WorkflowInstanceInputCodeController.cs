using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Serilog;

namespace GenericWorkflowAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class WorkflowInstanceInputCodeController : GenericOnlyGetAllController<WorkflowInstanceInputCode, WorkflowInstanceInputCodeDto>
    {
        public WorkflowInstanceInputCodeController(ILogger loggerManager, IMediator mediator)
            : base(loggerManager, mediator,
                  new List<string> {
                      $"{nameof(WorkflowInstanceInputCode.Instance)}",
                      $"{nameof(WorkflowInstanceInputCode.Type)}",
                      $"{nameof(WorkflowInstanceInputCode.ChangedByUser)}"
                  })
        {
        }

        //[EnableQuery]
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
    }
}