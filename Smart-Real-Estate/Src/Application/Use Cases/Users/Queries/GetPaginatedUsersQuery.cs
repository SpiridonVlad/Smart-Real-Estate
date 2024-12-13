using Application.DTOs;
using Application.Use_Cases.Filters;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetPaginatedUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public UserFilters Filters { get; set; } = new UserFilters();
    }
}
