namespace Application.Filters
{
    public class RecordFilter
    {
        public required ListingFilter ListingFilter { get; set; }
        public required PropertyFilter PropertyFilter { get; set; }
        public required AddressFilter AddressFilter { get; set; }
        public required UserFilter UserFilters { get; set; }
    }
}
