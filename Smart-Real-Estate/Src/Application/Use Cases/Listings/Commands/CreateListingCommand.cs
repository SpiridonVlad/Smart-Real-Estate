using Domain.Common;
using Domain.Entities.Features;
using MediatR;

namespace Application.Use_Cases.Commands
{
    public class CreateListingCommand : IRequest<Result<Guid>>
    {
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? Description { get; set; }
        public required ListingFeatures Features { get; set; }
    }
}
