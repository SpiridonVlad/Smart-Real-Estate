namespace Application.DTOs
{
    public class ListingDto
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsSold { get; set; }
        public bool IsHighlighted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
