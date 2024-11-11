using Domain.Common;
using MediatR;


namespace Application.Use_Cases.Users.Commands
{
    public class AddPropertyToHistoryCommand :  IRequest<Result<string>>
    {
        public Guid UserId { get; set; }
        public Guid PropertyId { get; set; }
    }
}
