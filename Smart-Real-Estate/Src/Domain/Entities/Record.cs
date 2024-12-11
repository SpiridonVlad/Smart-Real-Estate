using Domain.Entities.Cards;

namespace Domain.Entities
{
    public class Record
    {
        public required Address Address { get; set; }
        public required PropertyCard Property { get; set; }
        public required UserCard User { get; set; }
        public required ListingCard Listing { get; set; }
    }

}
