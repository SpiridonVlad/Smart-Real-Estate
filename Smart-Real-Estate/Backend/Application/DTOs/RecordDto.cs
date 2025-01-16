using Domain.Entities;
using Domain.Entities.Cards;

namespace Application.DTOs
{
    public class RecordDto
    {
        public required Address Address { get; set; }
        public required PropertyCard Property { get; set; }
        public required UserCard User { get; set; }
        public required ListingCard Listing { get; set; }
    }
}
