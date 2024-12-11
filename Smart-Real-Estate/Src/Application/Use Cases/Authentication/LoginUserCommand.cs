using Domain.Common;
using MediatR;

namespace Application.Authentication
{
    public class LoginUserCommand : IRequest<Result<string>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
