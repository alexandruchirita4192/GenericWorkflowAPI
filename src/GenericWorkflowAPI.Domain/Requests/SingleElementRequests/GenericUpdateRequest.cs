using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class GenericUpdateRequest<TDto> : IRequest<GenericApiResponse<string>>
        where TDto : class, IBaseDto, new()
    {
        public IdentityUser? User { get; set; }
        public TDto? Item { get; set; }
    }
}