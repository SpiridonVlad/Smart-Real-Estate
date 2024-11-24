using MediatR;
using Domain.Repositories;
using Domain.Common;
using Application.Use_Cases.Users.Commands;

namespace Application.Use_Cases.Users.CommandHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<string>>
    {
        private readonly IUserRepository repository;

        public DeleteUserCommandHandler(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await repository.DeleteAsync(request.Id);
            if (result.IsSuccess)
            {
                return Result<string>.Success("Deleted succesfully");
            }

            return Result<string>.Failure(result.ErrorMessage);
        }
    }
}