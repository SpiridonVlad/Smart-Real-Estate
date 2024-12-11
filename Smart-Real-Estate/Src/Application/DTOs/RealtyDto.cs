using Domain.Entities.Cards;

namespace Application.DTOs
{
    public class RealtyDto
    {
        public required PropertyCard Property { get; set; }
        public required AddressDto Address { get; set; }
    }
}
