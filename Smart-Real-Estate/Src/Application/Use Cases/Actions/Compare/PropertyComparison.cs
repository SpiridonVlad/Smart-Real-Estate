using Domain.Entities;
using Domain.Types;

namespace Domain.Comparison
{
    public class PropertyComparison
    {
        public required Property BaseProperty { get; set; }
        public required Property ComparedProperty { get; set; }

        public double OverallSimilarityScore { get; set; }
        public Dictionary<PropertyFeatureType, ComparisonDetail> FeatureComparisons { get; set; }
        public AddressProximityAnalysis? AddressProximity { get; set; }
        public TypeCompatibilityScore? TypeCompatibility { get; set; }

        public static PropertyComparison Compare(Property baseProperty, Property comparedProperty)
        {
            var comparison = new PropertyComparison
            {
                BaseProperty = baseProperty,
                ComparedProperty = comparedProperty,

                TypeCompatibility = ComparePropertyTypes(baseProperty.Type, comparedProperty.Type),

                FeatureComparisons = CompareFeatures(baseProperty.Features, comparedProperty.Features),

            };

            comparison.CalculateOverallSimilarityScore();

            return comparison;
        }

        private void CalculateOverallSimilarityScore()
        {
            double typeScore = TypeCompatibility.Score * 0.2;
            double featureScore = CalculateFeatureSimilarityScore() * 0.5;
            double addressScore = AddressProximity.SimilarityScore * 0.3;

            OverallSimilarityScore = Math.Round(typeScore + featureScore + addressScore, 2);
        }

        private double CalculateFeatureSimilarityScore()
        {
            if (FeatureComparisons == null || FeatureComparisons.Count == 0)
                return 0;

            var criticalFeatureWeights = new Dictionary<PropertyFeatureType, double>
            {
                { PropertyFeatureType.Rooms, 0.15 },
                { PropertyFeatureType.Surface, 0.15 },
                { PropertyFeatureType.Floor, 0.1 },
                { PropertyFeatureType.Year, 0.1 },
                { PropertyFeatureType.HeatingUnit, 0.1 },
                { PropertyFeatureType.Parking, 0.1 }
                // etc
            };

            double weightedSimilarityScore = FeatureComparisons
                .Sum(fc =>
                {
                    double weight = criticalFeatureWeights.TryGetValue(fc.Key, out double value)
                        ? value : 0.05;
                    return fc.Value.SimilarityScore * weight;
                });

            return Math.Round(weightedSimilarityScore, 2);
        }

        private static TypeCompatibilityScore ComparePropertyTypes(PropertyType type, PropertyType compared)
        {
            var compatibilityMatrix = new Dictionary<(PropertyType, PropertyType), double>
            {
                { (PropertyType.Apartment, PropertyType.Studio), 0.8 },
                { (PropertyType.Studio, PropertyType.Apartment), 0.8 },
                { (PropertyType.House, PropertyType.Apartment), 0.5 },
                { (PropertyType.Office, PropertyType.CommercialSpace), 0.7 }
            };

            var key = (type, compared);
            var reverseKey = (compared, type);

            double score = compatibilityMatrix.TryGetValue(key, out double value)
                ? value : (compatibilityMatrix.ContainsKey(reverseKey)
                    ? compatibilityMatrix[reverseKey]
                    : (type == compared ? 1.0 : 0.3));

            return new TypeCompatibilityScore
            {
                BaseType = type,
                ComparedType = compared,
                Score = score
            };
        }

        private static Dictionary<PropertyFeatureType, ComparisonDetail> CompareFeatures(
            Dictionary<PropertyFeatureType, int> baseFeatures,
            Dictionary<PropertyFeatureType, int> comparedFeatures)
        {
            var comparisons = new Dictionary<PropertyFeatureType, ComparisonDetail>();

            var allFeatureTypes = baseFeatures.Keys.Union(comparedFeatures.Keys);

            foreach (var featureType in allFeatureTypes)
            {
                int baseValue = baseFeatures.TryGetValue(featureType, out var bv) ? bv : 0;
                int comparedValue = comparedFeatures.TryGetValue(featureType, out var cv) ? cv : 0;

                comparisons[featureType] = new ComparisonDetail
                {
                    FeatureType = featureType,
                    BaseValue = baseValue,
                    ComparedValue = comparedValue,
                    SimilarityScore = CalculateFeatureSimilarity(featureType, baseValue, comparedValue)
                };
            }

            return comparisons;
        }

        private static double CalculateFeatureSimilarity(PropertyFeatureType featureType, int baseValue, int comparedValue)
        {
            switch (featureType)
            {
                case PropertyFeatureType.Rooms:
                case PropertyFeatureType.Surface:
                    return Math.Max(0, 1 - Math.Abs(baseValue - comparedValue) / (double)Math.Max(baseValue, comparedValue));

                case PropertyFeatureType.Floor:
                case PropertyFeatureType.Year:
                    return 1 - Math.Min(Math.Abs(baseValue - comparedValue) / 10.0, 1);

                case PropertyFeatureType.Parking:
                case PropertyFeatureType.Garage:
                case PropertyFeatureType.Elevator:
                    return baseValue == comparedValue ? 1.0 : 0.0;

                default:
                    return baseValue == comparedValue ? 1.0 : 0.5;
            }
        }
    }

    public class TypeCompatibilityScore
    {
        public PropertyType BaseType { get; set; }
        public PropertyType ComparedType { get; set; }
        public double Score { get; set; }
    }

    public class ComparisonDetail
    {
        public PropertyFeatureType FeatureType { get; set; }
        public int BaseValue { get; set; }
        public int ComparedValue { get; set; }
        public double SimilarityScore { get; set; }
    }

    public class AddressProximityAnalysis
    {
        public double DistanceInKilometers { get; set; }
        public double SimilarityScore { get; set; }
    }
}