using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class GenericDeleteRequest<TDto> : IRequest<GenericApiResponse<string>>
        where TDto : class, IBaseDto, new()
    {
        public string Code { get; set; }
    }
}