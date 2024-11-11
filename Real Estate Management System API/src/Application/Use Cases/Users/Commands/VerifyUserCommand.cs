using Domain.Common;
using MediatR;


namespace Application.Use_Cases.Users.Commands
{
    public class VerifyUserCommand : IRequest<Result<string>>
    {
        public Guid UserId { get; set; }
    
    }
}
