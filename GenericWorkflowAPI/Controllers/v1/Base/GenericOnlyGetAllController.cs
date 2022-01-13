using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Core.Attributes;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.Controllers.v1
{
    [Authorize]
    [SerilogLogging]
    [Route("api/v{version:ApiVersion}/[controller]s")]
    public abstract class GenericOnlyGetAllController<TEntity, TDto> : ControllerBase
    where TEntity : class, IIdEntity, new()
    where TDto : class, IBaseDto, new()
    {
        protected readonly ILogger _logger;
        protected readonly IMediator _mediator;

        protected readonly List<string> _includePathList;

        public GenericOnlyGetAllController(ILogger logger, IMediator mediator, List<string> includePathList)
        {
            _logger = logger;
            _mediator = mediator;
            _includePathList = includePathList;
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<IActionResult> GetCollection(CancellationToken cancellationToken)
        {
            var request = new GenericGetListRequest<TDto> { IncludePathList = _includePathList };
            try
            {
                request.User = this.GetUser();
                var response = await _mediator.Send(request, cancellationToken);
                return await _mediator.Send(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericOnlyGetAllController<TEntity, TDto>).FullName}.{nameof(GetCollection)}({JsonConvert.SerializeObject(request)})");
                return Problem(ValidationConstants.GenericValidationMessage, statusCode: 500);
            }
        }
    }
}