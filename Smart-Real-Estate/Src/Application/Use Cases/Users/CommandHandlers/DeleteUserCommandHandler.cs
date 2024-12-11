using MediatR;
using Domain.Repositories;
using Domain.Common;
using Application.Use_Cases.Users.Commands;

namespace Application.Use_Cases.Users.CommandHandlers
{
    public class DeleteUserCommandHandler(IUserRepository repository) : IRequestHandler<DeleteUserCommand, Result<string>>
    {
        private readonly IUserRepository repository = repository;

        public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await repository.DeleteAsync(request.Id);
            if (result.IsSuccess)
            {
                return Result<string>.Success("");
            }

            return Result<string>.Failure(result.ErrorMessage);
        }
    }
}