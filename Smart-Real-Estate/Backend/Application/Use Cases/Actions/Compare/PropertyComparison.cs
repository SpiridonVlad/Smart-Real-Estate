using Domain.Types;

namespace Domain.Comparison
{
    public class PropertyComparison
    {
        public double OverallSimilarityScore { get; set; }
        public Dictionary<PropertyFeatureType, ComparisonDetail> FeatureComparisons { get; set; } = [];
        public AddressProximityAnalysis? AddressProximity { get; set; }
        public TypeCompatibilityScore? TypeCompatibility { get; set; }
        public string WinningProperty { get; set; } = string.Empty; // Identifies the better property
        public List<string> Reasons { get; set; } = []; // Explanation of why one is better
    }

    public class ComparisonDetail
    {
        public PropertyFeatureType FeatureType { get; set; }
        public int BaseValue { get; set; }
        public int ComparedValue { get; set; }
        public double SimilarityScore { get; set; }
    }

    public class TypeCompatibilityScore
    {
        public PropertyType BaseType { get; set; }
        public PropertyType ComparedType { get; set; }
        public double Score { get; set; }
    }

    public class AddressProximityAnalysis
    {
        public double DistanceInKilometers { get; set; }
        public double SimilarityScore { get; set; }
    }
}
