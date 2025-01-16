using Domain.Entities;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Filters
{
    public class ListingFilter
    {
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public DateTime? MinPublicationDate { get; set; }
        public DateTime? MaxPublicationDate { get; set; }
        public string? ListingDescriptionContains { get; set; }
        public Dictionary<ListingType, int>? ListingFeatures { get; set; }

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

            if (ListingFeatures != null && ListingFeatures.Count != 0)
            {
                foreach (var feature in ListingFeatures)
                {
                    var featureKey = feature.Key.ToString();
                    var featureValue = feature.Value;

                    filter = filter.And(r => EF.Functions.JsonContains(r.Features, $"{{\"{featureKey}\":{featureValue}}}"));
                }
            }

            return filter;
        }
    }
}
