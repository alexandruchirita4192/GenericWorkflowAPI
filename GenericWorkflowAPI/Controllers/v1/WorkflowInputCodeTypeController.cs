using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class WorkflowInputCodeTypeController : GenericCRUDController<WorkflowInputCodeType, WorkflowInputCodeTypeDto>
    {
        public WorkflowInputCodeTypeController(ILogger loggerManager, IMediator mediator)
            : base(loggerManager, mediator,
                  new List<string> {
                      $"{nameof(WorkflowInputCodeType.Workflow)}",
                      $"{nameof(WorkflowInputCodeType.ChangedByUser)}"
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
            return await base.GetItem(code, cancellationToken);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public new Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return base.GetCollection(cancellationToken);
        }

        [EnableQuery]
        [HttpGet("Queryable")]
        //[ODataRoute("Queryable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IQueryable<WorkflowInputCodeTypeDto>> Get([OpenApiParameterIgnore] ODataQueryOptions<WorkflowInputCodeTypeDto> queryOptions, CancellationToken cancellationToken)
        {
            return await base.GetQueryable(queryOptions, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{code}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] WorkflowInputCodeTypeDto item, CancellationToken cancellationToken)
        {
            return await base.CreateItem(item, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] Collection<WorkflowInputCodeTypeDto> collection, CancellationToken cancellationToken)
        {
            return await base.CreateCollection(collection, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] WorkflowInputCodeTypeDto item, CancellationToken cancellationToken)
        {
            return await base.UpdateItem(item, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Collection<WorkflowInputCodeTypeDto> collection, CancellationToken cancellationToken)
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