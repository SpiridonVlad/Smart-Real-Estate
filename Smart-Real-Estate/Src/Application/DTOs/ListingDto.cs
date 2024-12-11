using Domain.Entities.Features;

namespace Application.DTOs
{
    public class ListingDto
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? Description { get; set; }
        public required ListingFeatures Features { get; set; }
    }
}
