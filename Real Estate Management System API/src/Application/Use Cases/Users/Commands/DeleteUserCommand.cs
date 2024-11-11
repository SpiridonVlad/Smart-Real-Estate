using Domain.Common;
using MediatR;


namespace Application.Use_Cases.Users.Commands
{
    public class DeleteUserCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
    }
}
