using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkflowAPI.Domain.Constants;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Entities;
using GenericWorkflowAPI.Domain.Requests;
using GenericWorkflowAPI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace GenericWorkflowAPI.Controllers.v1
{
    public abstract class GenericGetController<TEntity, TDto> : GenericOnlyGetAllController<TEntity, TDto>
        where TEntity : class, IBaseEntity, new()
        where TDto : class, IBaseDto, new()
    {
        public GenericGetController(ILogger _logger, IMediator _mediator, List<string> _includePathList)
            : base(_logger, _mediator, _includePathList)
        {
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<IActionResult> GetItem(string code, CancellationToken cancellationToken)
        {
            var request = new GenericGetRequest<TDto>() { Code = code, IncludePathList = _includePathList };
            try
            {
                request.User = this.GetUser();
                var response = await _mediator.Send(request, cancellationToken);
                return await _mediator.Send(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericGetController<TEntity, TDto>).FullName}.{nameof(GetCollection)}({JsonConvert.SerializeObject(request)})");
                return Problem(ValidationConstants.GenericValidationMessage, statusCode: 500);
            }
        }
    }
}