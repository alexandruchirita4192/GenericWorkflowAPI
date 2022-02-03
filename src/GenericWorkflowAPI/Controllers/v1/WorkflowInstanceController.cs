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
    public class WorkflowInstanceController : GenericGetController<WorkflowInstance, WorkflowInstanceDto>
    {
        public WorkflowInstanceController(ILogger loggerManager, IMediator mediator)
            : base(loggerManager, mediator,
                  new List<string> {
                      nameof(WorkflowInstance.Workflow),
                      nameof(WorkflowInstance.CurrentState),
                      nameof(WorkflowInstance.ChangedByUser)
                  })
        {
        }

        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string code, CancellationToken cancellationToken)
        {
            return await GetItem(code, cancellationToken);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return await GetCollection(cancellationToken);
        }

        [EnableQuery]
        [HttpGet("Queryable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IQueryable<WorkflowInstanceDto>> Get([OpenApiParameterIgnore] ODataQueryOptions<WorkflowInstanceDto> queryOptions, CancellationToken cancellationToken)
        {
            return await GetQueryable(queryOptions, cancellationToken);
        }
    }
}