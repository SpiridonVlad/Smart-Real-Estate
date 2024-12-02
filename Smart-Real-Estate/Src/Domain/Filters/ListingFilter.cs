using Domain.Types;

namespace Domain.Filters
{
    public class ListingFilter
    {
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public PropertyType? Type { get; set; }

    }
}
