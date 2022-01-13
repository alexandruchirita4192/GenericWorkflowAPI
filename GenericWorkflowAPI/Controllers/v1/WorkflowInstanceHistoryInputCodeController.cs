using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Attributes;
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
    public class WorkflowInstanceHistoryInputCodeController : GenericOnlyGetAllController<WorkflowInstanceHistoryInputCode, WorkflowInstanceHistoryInputCodeDto>
    {
        public WorkflowInstanceHistoryInputCodeController(ILogger loggerManager, IMediator mediator)
            : base(loggerManager, mediator,
                  new List<string> {
                      $"{nameof(WorkflowInstanceHistoryInputCode.History)}",
                      $"{nameof(WorkflowInstanceHistoryInputCode.InputCodeType)}",
                      $"{nameof(WorkflowInstanceHistoryInputCode.ChangedByUser)}"
                  })
        {
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
        //[ODataRoute("Queryable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IQueryable<WorkflowInstanceHistoryInputCodeDto>> Get([OpenApiParameterIgnore] ODataQueryOptions<WorkflowInstanceHistoryInputCodeDto> queryOptions, CancellationToken cancellationToken)
        {
            return await base.GetQueryable(queryOptions, cancellationToken);
        }
    }
}