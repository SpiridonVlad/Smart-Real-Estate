using MediatR;

namespace Application.Use_Cases.Commands
{
    public class UpdateUserCommand : CreateUserCommand, IRequest
    {
        public Guid Id { get; set; }
    }
}
