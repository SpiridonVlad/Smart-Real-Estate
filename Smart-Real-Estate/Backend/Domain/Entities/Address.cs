using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Address
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public string? PostalCode { get; set; }
        public required string Country { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
