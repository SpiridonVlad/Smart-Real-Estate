using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands
{
    public class CreateUserCommand : IRequest<Result<Guid>>
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public bool Verified { get; set; }
        public decimal Rating { get; set; }
        public bool IsAdmin { get; set; }
    }
}