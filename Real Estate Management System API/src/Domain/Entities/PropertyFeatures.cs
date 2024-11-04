using Domain.Types;

namespace Domain.Entities
{
    public class PropertyFeatures
    {
        public Dictionary<PropertyFeatureType, int> Features { get; set; } = new Dictionary<PropertyFeatureType, int>();

        public int? GetFeatureSquareMeters(PropertyFeatureType featureType)
        {
            return Features.TryGetValue(featureType, out int squareMeters) ? squareMeters : (int?)null;
        }
    }
}
