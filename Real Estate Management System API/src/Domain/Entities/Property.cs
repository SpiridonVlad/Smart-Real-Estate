namespace Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public int Surface { get; set; }
        public int Rooms { get; set; }
        public string Image { get; set; }
        public bool IsApartament { get; set; }
        public bool HasGarden { get; set; }
        public bool HasGarage { get; set; }
        public bool HasPool { get; set; }
        public bool HasBalcony { get; set; }



    }
}
