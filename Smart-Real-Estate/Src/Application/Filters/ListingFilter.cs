using Domain.Entities;
using Domain.Types;
using System.Linq.Expressions;

namespace Application.Filters
{
    public class ListingFilter
    {
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public DateTime? MinPublicationDate { get; set; }
        public DateTime? MaxPublicationDate { get; set; }
        public string? ListingDescriptionContains { get; set; }
        public Dictionary<ListingAssetss, int>? ListingFeaturesMinValues { get; set; }
        public Dictionary<ListingAssetss, int>? ListingFeaturesMaxValues { get; set; }

        public Expression<Func<Listing, bool>> BuildFilterExpression()
        {
            Expression<Func<Listing, bool>> filter = r => true;

            if (MinPrice.HasValue)
                filter = filter.And(r => r.Price >= MinPrice.Value);

            if (MaxPrice.HasValue)
                filter = filter.And(r => r.Price <= MaxPrice.Value);

            if (MinPublicationDate.HasValue)
                filter = filter.And(r => r.PublicationDate >= MinPublicationDate.Value);

            if (MaxPublicationDate.HasValue)
                filter = filter.And(r => r.PublicationDate <= MaxPublicationDate.Value);

            if (!string.IsNullOrWhiteSpace(ListingDescriptionContains))
                filter = filter.And(r => r.Description.Contains(ListingDescriptionContains));

            if (ListingFeaturesMinValues != null)
            {
                foreach (var feature in ListingFeaturesMinValues)
                {
                    filter = filter.And(r =>
                        r.Features.Features.ContainsKey(feature.Key) &&
                        r.Features.Features[feature.Key] >= feature.Value);
                }
            }

            if (ListingFeaturesMaxValues != null)
            {
                foreach (var feature in ListingFeaturesMaxValues)
                {
                    filter = filter.And(r =>
                        r.Features.Features.ContainsKey(feature.Key) &&
                        r.Features.Features[feature.Key] <= feature.Value);
                }
            }
            return filter;
        }
    }
}
