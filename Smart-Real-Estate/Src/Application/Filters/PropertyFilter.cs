using Domain.Entities;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
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

            //if (PropertyFeaturesMinValues != null)
            //{
            //    foreach (var feature in PropertyFeaturesMinValues)
            //    {
            //        var featureKey = feature.Key.ToString();
            //        var featureValue = feature.Value;

            //        filter = filter.And(r =>
            //            EF.Functions.JsonContains(r.Features, $"{{\"{featureKey}\":{{\"$gte\":{featureValue}}}}}}}"));
            //    }
            //}

            //if (PropertyFeaturesMaxValues != null)
            //{
            //    foreach (var feature in PropertyFeaturesMaxValues)
            //    {
            //        var featureKey = feature.Key.ToString();
            //        var featureValue = feature.Value;

            //        filter = filter.And(r =>
            //            EF.Functions.JsonContains(r.Features, $"{{\"{featureKey}\":{{\"$lte\":{featureValue}}}}}}}"));
            //    }
            //}

            return filter;
        }

    }
}
