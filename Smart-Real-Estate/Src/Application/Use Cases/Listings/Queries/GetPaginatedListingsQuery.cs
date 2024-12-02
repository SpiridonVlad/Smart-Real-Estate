using Application.DTOs;
using Domain.Common;
using Domain.Filters;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetPaginatedListingsQuery: IRequest<Result<IEnumerable<ListingDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public ListingFilter? Filter { get; set; }
    }
}
