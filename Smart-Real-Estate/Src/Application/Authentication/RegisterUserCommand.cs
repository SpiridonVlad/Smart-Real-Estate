using Domain.Common;
using Domain.Types;
using MediatR;

namespace Application.Authentication
{
    public class RegisterUserCommand : IRequest<Result<string>>
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public UserType Type { get; set; }
    }
}
