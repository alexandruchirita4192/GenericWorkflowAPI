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
    public class WorkflowStateInputCodeTypeController : GenericCRUDController<WorkflowStateInputCodeType, WorkflowStateInputCodeTypeDto>
    {
        public WorkflowStateInputCodeTypeController(ILogger loggerManager, IMediator mediator)
            : base(loggerManager, mediator,
                  new List<string> {
                      nameof(WorkflowStateInputCodeType.State),
                      nameof(WorkflowStateInputCodeType.InputCodeType),
                      nameof(WorkflowStateInputCodeType.ChangedByUser)
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
        public async Task<IQueryable<WorkflowStateInputCodeTypeDto>> Get([OpenApiParameterIgnore] ODataQueryOptions<WorkflowStateInputCodeTypeDto> queryOptions, CancellationToken cancellationToken)
        {
            return await GetQueryable(queryOptions, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{code}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] WorkflowStateInputCodeTypeDto item, CancellationToken cancellationToken)
        {
            return await CreateItem(item, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] Collection<WorkflowStateInputCodeTypeDto> collection, CancellationToken cancellationToken)
        {
            return await CreateCollection(collection, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] WorkflowStateInputCodeTypeDto item, CancellationToken cancellationToken)
        {
            return await UpdateItem(item, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] Collection<WorkflowStateInputCodeTypeDto> collection, CancellationToken cancellationToken)
        {
            return await UpdateCollection(collection, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpDelete("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string code, CancellationToken cancellationToken)
        {
            return await DeleteItem(code, cancellationToken);
        }

        [ValidateAntiForgeryToken]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] Collection<string> codes, CancellationToken cancellationToken)
        {
            return await DeleteCollection(codes, cancellationToken);
        }
    }
}