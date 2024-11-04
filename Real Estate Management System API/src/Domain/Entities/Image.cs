namespace Domain.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public string Description { get; set; }
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
