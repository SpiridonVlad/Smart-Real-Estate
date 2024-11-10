using Domain.Types;

namespace Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public Guid AdressId { get; set; }
        public Address Address { get; set; }
        public string ImageId { get; set; } 
        public Guid UserId { get; set; }
        public User User { get; set; }
        public PropertyType Type { get; set; }
        public PropertyFeatures Features { get; set; }

    }
}
