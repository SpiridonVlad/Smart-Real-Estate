using Domain.Types;

namespace Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public int Surface { get; set; }
        public int Rooms { get; set; }
        public string ImageId { get; set; } 
        public Guid UserId { get; set; }
        public User User { get; set; }
        public PropertyType Type { get; set; }
        public bool HasGarden { get; set; }
        public bool HasGarage { get; set; }
        public bool HasPool { get; set; }
        public bool HasBalcony { get; set; }

    }
}
