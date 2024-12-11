using Domain.Entities.Features;

namespace Domain.Entities
{
    public class Listing
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public required ListingFeatures Features { get; set; }
    }
}
