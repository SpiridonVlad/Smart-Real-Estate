using Domain.Entities.Features;
using Domain.Types;

namespace Domain.Entities.Cards
{
    public class PropertyCard
    {
        public required string ImageId { get; set; }
        public PropertyType PType { get; set; }
        public required PropertyFeatures PFeatures { get; set; }
    }
}
