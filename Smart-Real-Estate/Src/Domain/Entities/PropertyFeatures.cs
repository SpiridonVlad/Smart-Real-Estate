using Domain.Types;

namespace Domain.Entities
{
    public class PropertyFeatures
    {
        public Dictionary<PropertyFeatureType, int> Features { get; set; } = new Dictionary<PropertyFeatureType, int>();
    }
}
