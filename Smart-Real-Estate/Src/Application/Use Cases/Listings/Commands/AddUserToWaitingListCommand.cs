using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Listings.Commands
{
    public class AddUserToWaitingListCommand : IRequest<Result<string>>
    {
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }
    }
}
