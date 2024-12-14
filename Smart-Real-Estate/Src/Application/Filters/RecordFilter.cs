namespace Application.Filters
{
    public class RecordFilter
    {
        public required ListingFilter ListingFilter { get; set; } = new ListingFilter();
        public required PropertyFilter PropertyFilter { get; set; } = new PropertyFilter();
        public required AddressFilter AddressFilter { get; set; }= new AddressFilter();
        public required UserFilter UserFilters { get; set; } = new UserFilter();
    }
}
