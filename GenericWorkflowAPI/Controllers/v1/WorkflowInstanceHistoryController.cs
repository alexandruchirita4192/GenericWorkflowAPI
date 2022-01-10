using System.Collections.Generic;
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
    public class WorkflowInstanceHistoryController : GenericOnlyGetAllController<WorkflowInstanceHistory, WorkflowInstanceHistoryDto>
    {
        public WorkflowInstanceHistoryController(ILogger loggerManager, IMediator mediator)
            : base(loggerManager, mediator,
                  new List<string> {
                      $"{nameof(WorkflowInstanceHistory.Instance)}",
                      $"{nameof(WorkflowInstanceHistory.CurrentState)}",
                      $"{nameof(WorkflowInstanceHistory.NextState)}",
                      $"{nameof(WorkflowInstanceHistory.ChangedByUser)}"
                  })
        {
        }

        //[EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return await base.GetCollection(cancellationToken);
        }
    }
}