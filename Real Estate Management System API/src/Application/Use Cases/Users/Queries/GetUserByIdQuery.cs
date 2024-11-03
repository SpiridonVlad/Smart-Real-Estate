using Application.DTOs;
using Domain.Common;
using MediatR;


namespace Application.Use_Cases.Queries
{
    public class GetUserByIdQuery : IRequest<Result<UserDto>>
    {
        public Guid Id { get; set; }
    }
}
