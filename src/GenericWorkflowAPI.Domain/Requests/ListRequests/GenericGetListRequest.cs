using System.Collections.Generic;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class GenericGetListRequest<TDto> : IRequest<GenericApiResponse<List<TDto>>>
        where TDto : class, IBaseDto, new()
    {
        public List<string>? IncludePathList { get; set; }
    }
}