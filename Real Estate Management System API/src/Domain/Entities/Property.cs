using Domain.Types;

namespace Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public Address Address { get; set; }
        public Guid AddressId { get; set; }
        public int Surface { get; set; }
        public int Rooms { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public PropertyType Type { get; set; }
        public PropertyFeatures Features { get; set; } = new PropertyFeatures();
    }
}
