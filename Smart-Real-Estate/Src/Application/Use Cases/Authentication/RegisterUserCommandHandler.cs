using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Authentication
{
    public class RegisterUserCommandHandler(IUserRepository repository) : IRequestHandler<RegisterUserCommand, Result<string>>
    {
        private readonly IUserRepository userRepository = repository;

        public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Username = request.Username,
            };
            var token = await userRepository.Register(user,cancellationToken);

            if(token.IsSuccess)
            {
                return Result<string>.Success("User registered successfully");
            }
            return Result<string>.Failure("User already exists");
        }
    }
}
