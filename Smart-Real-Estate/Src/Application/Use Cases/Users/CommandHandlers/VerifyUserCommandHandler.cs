using Application.Use_Cases.Users.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Users.CommandHandlers
{
    public class VerifyUserCommandHandler : IRequestHandler<VerifyUserCommand, Result<string>>
    {
        private readonly IUserRepository repository;
        public VerifyUserCommandHandler(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
        }
        public async Task<Result<string>> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
        {
            var userResult = await repository.GetByIdAsync(request.UserId);
            if (!userResult.IsSuccess)
            {
                return Result<string>.Failure("User not found");
            }
            var user = userResult.Data;
            user.Verified = true;
            var updateResult = await repository.UpdateAsync(user);
            if (updateResult.IsSuccess)
            {
                return Result<string>.Success("User verified successfully");
            }
            return Result<string>.Failure(updateResult.ErrorMessage);

        }
    }
}
