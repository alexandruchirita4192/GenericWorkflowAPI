using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.OData.Query;

namespace GenericWorkflowAPI.Domain.Requests
{
    public class GenericGetQueryableRequest<TDto> : IRequest<IQueryable<TDto>>
    {
        public ODataQueryOptions<TDto>? QueryOptions { get; set; }
        public List<string>? IncludePathList { get; set; }
    }
}