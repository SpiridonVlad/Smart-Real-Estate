using Domain.Entities.Cards;

namespace Domain.Entities
{
    public class Realty
    {
        public required PropertyCard Property { get; set; }
        public required Address Address { get; set; }
    }
}
