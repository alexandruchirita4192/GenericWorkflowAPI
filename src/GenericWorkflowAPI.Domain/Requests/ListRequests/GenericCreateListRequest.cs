using System.Collections.ObjectModel;
using GenericWorkflowAPI.Domain.DTOs;
using GenericWorkflowAPI.Domain.Responses;
using MediatR;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class GenericCreateListRequest<TDto> : IRequest<GenericApiResponse<string>>
        where TDto : class, IBaseDto, new()
    {
        public IdentityUser? User { get; set; }
        public Collection<TDto>? Collection { get; set; }
    }
}