using Domain.Entities.Features;
using Domain.Types;

namespace Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public required Address Address { get; set; }
        public required string ImageId { get; set; } 
        public Guid UserId { get; set; }
        public required User User { get; set; }
        public PropertyType Type { get; set; }
        public required PropertyFeatures Features { get; set; }

    }
}
