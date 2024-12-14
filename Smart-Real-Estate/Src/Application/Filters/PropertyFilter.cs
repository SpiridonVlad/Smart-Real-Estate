using Domain.Entities;
using Domain.Types;
using System.Linq.Expressions;

namespace Application.Filters
{
    public class PropertyFilter
    {
        public PropertyType? PropertyType { get; set; }
        public Dictionary<PropertyFeatureType, int>? PropertyFeaturesMinValues { get; set; }
        public Dictionary<PropertyFeatureType, int>? PropertyFeaturesMaxValues { get; set; }
        public Expression<Func<Property, bool>> BuildFilterExpression()
        {
            Expression<Func<Property, bool>> filter = r => true;

            if (PropertyType.HasValue)
                filter = filter.And(r => r.Type == PropertyType.Value);

            if (PropertyFeaturesMinValues != null)
            {
                foreach (var feature in PropertyFeaturesMinValues)
                {
                    filter = filter.And(r =>
                        r.Features.Features.ContainsKey(feature.Key) &&
                        r.Features.Features[feature.Key] >= feature.Value);
                }
            }

            if (PropertyFeaturesMaxValues != null)
            {
                foreach (var feature in PropertyFeaturesMaxValues)
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
