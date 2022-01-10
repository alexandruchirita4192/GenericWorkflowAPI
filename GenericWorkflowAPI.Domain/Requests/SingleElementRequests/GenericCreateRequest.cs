using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class GenericCreateRequest<TDto> : IRequest<GenericApiResponse<string>>
        where TDto : class, IBaseDto, new()
    {
        public TDto Item { get; set; }
    }
}