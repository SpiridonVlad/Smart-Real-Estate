using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetAllUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
