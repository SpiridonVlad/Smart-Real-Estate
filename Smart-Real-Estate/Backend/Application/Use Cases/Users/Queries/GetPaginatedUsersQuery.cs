using Application.DTOs;
using Application.Filters;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetPaginatedUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public UserFilter Filters { get; set; } = new UserFilter();
    }
}
