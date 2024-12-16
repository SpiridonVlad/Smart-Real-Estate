

using System.ComponentModel.DataAnnotations.Schema;

namespace Application.AIML
{
    public class PropertyData
    {
        [Column(TypeName = "int")]
        public int Surface { get; set; }
        public int Rooms { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
        public int Year { get; set; }
        public bool Parking { get; set; }
        public int Floor { get; set; }

    }
}
