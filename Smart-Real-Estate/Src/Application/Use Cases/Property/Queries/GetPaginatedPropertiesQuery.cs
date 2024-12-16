using Application.DTOs;
using Application.Filters;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetPaginatedPropertiesQuery : IRequest<Result<IEnumerable<PropertyDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public PropertyFilter Filter { get; set; } = new PropertyFilter();
    }
}
