using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public abstract class GenericCRUDController<TEntity, TDto> : GenericGetController<TEntity, TDto>
        where TEntity : class, IBaseEntity, new()
        where TDto : class, IBaseDto, new()
    {
        public GenericCRUDController(ILogger _logger, IMediator _mediator, List<string> _includePathList)
            : base(_logger, _mediator, _includePathList)
        {
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<IActionResult> CreateItem(TDto item, CancellationToken cancellationToken)
        {
            var request = new GenericCreateRequest<TDto>() { Item = item };
            try
            {
                request.User = this.GetUser();
                var response = await _mediator.Send(request, cancellationToken);
                return await _mediator.Send(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericCRUDController<TEntity, TDto>).FullName}.{nameof(CreateItem)}({JsonConvert.SerializeObject(request)})");
                return Problem(ValidationConstants.GenericValidationMessage, statusCode: 500);
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<IActionResult> CreateCollection(Collection<TDto> collection, CancellationToken cancellationToken)
        {
            var request = new GenericCreateListRequest<TDto>() { Collection = collection };
            try
            {
                request.User = this.GetUser();
                var response = await _mediator.Send(request, cancellationToken);
                return await _mediator.Send(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericCRUDController<TEntity, TDto>).FullName}.{nameof(CreateCollection)}({JsonConvert.SerializeObject(request)})");
                return Problem(ValidationConstants.GenericValidationMessage, statusCode: 500);
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<IActionResult> UpdateItem(TDto item, CancellationToken cancellationToken)
        {
            var request = new GenericUpdateRequest<TDto>() { Item = item };
            try
            {
                request.User = this.GetUser();
                var response = await _mediator.Send(request, cancellationToken);
                return await _mediator.Send(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericCRUDController<TEntity, TDto>).FullName}.{nameof(UpdateItem)}({JsonConvert.SerializeObject(request)})");
                return Problem(ValidationConstants.GenericValidationMessage, statusCode: 500);
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<IActionResult> UpdateCollection(Collection<TDto> collection, CancellationToken cancellationToken)
        {
            var request = new GenericUpdateListRequest<TDto>() { Collection = collection };
            try
            {
                request.User = this.GetUser();
                var response = await _mediator.Send(request, cancellationToken);
                return await _mediator.Send(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericCRUDController<TEntity, TDto>).FullName}.{nameof(UpdateCollection)}({JsonConvert.SerializeObject(request)})");
                return Problem(ValidationConstants.GenericValidationMessage, statusCode: 500);
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<IActionResult> DeleteItem(string code, CancellationToken cancellationToken)
        {
            var request = new GenericDeleteRequest<TDto>() { Code = code };
            try
            {
                request.User = this.GetUser();
                var response = await _mediator.Send(request, cancellationToken);
                return await _mediator.Send(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericCRUDController<TEntity, TDto>).FullName}.{nameof(DeleteItem)}({JsonConvert.SerializeObject(request)})");
                return Problem(ValidationConstants.GenericValidationMessage, statusCode: 500);
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<IActionResult> DeleteCollection(Collection<string> codes, CancellationToken cancellationToken)
        {
            var request = new GenericDeleteListRequest<TDto>() { Codes = codes };
            try
            {
                request.User = this.GetUser();
                var response = await _mediator.Send(request, cancellationToken);
                return await _mediator.Send(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"{typeof(GenericCRUDController<TEntity, TDto>).FullName}.{nameof(DeleteCollection)}({JsonConvert.SerializeObject(request)})");
                return Problem(ValidationConstants.GenericValidationMessage, statusCode: 500);
            }
        }
    }
}