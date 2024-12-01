using Domain.Types;

namespace Domain.Entities
{
    public class ListingFeatures
    {
        public Dictionary<ListingAssetss, int> Features { get; set; } = new Dictionary<ListingAssetss, int>();
    }
}
