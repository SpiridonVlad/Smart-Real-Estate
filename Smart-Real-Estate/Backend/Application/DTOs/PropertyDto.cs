using Domain.Entities;
using Domain.Types;

namespace Application.DTOs
{
    public class PropertyDto
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public required string Title { get; set; }
        public required Address Address { get; set; }
        public required List<string> ImageIds { get; set; }
        public Guid UserId { get; set; } 
        public PropertyType Type { get; set; }
        public Dictionary<PropertyFeatureType, int> Features { get; set; } = [];
    }

}
