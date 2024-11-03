
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Listings.Commands
{
    public class DeleteListingCommand: IRequest<Result<string>>
    {
        public Guid Id { get; set; }
    }
}
