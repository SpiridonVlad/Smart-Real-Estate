using Domain.Entities.Features;

namespace Domain.Entities.Cards
{
    public class ListingCard
    {
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public required ListingFeatures LFeatures { get; set; }
    }
}
