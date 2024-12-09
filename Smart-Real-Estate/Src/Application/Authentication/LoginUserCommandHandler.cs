using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Org.BouncyCastle.Crypto.Generators;

namespace Application.Authentication
{
    public class LoginUserCommandHandler(IUserRepository repository) : IRequestHandler<LoginUserCommand, Result<string>>
    {
        private readonly IUserRepository userRepository = repository;

        public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Username = "",
            };
            var token = await userRepository.Login(user);
            if (token.IsSuccess)
            {
                return Result<string>.Success("User logged in successfully");
            }
            return Result<string>.Failure("Invalid email or password");
        }

    }
}
