using System.Collections.Generic;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class GenericGetRequest<TDto> : IRequest<GenericApiResponse<TDto>>
        where TDto : class, IBaseDto, new()
    {
        public IdentityUser User { get; set; }
        public string Code { get; set; }
        public List<string> IncludePathList { get; set; }
    }
}