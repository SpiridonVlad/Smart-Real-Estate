using Domain.Comparison;
using Domain.Repositories;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Domain.Types;

namespace Application.Use_Cases.Actions
{
    public class CompareTwoPropertiesCommandHandler(IPropertyRepository propertyRepository) : IRequestHandler<CompareTwoPropertiesCommand, Result<PropertyComparison>>
    {
        private readonly IPropertyRepository propertyRepository = propertyRepository;

        public async Task<Result<PropertyComparison>> Handle(CompareTwoPropertiesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var basePropertyResult = await propertyRepository.GetByIdAsync(request.Initial);
                var comparedPropertyResult = await propertyRepository.GetByIdAsync(request.Secondary);

                if (!basePropertyResult.IsSuccess || !comparedPropertyResult.IsSuccess)
                {
                    return Result<PropertyComparison>.Failure("One or both properties not found.");
                }

                var baseProperty = basePropertyResult.Data;
                var comparedProperty = comparedPropertyResult.Data;

                var result = new PropertyComparison
                {
                    TypeCompatibility = ComparePropertyTypes(baseProperty.Type, comparedProperty.Type),
                    FeatureComparisons = CompareFeatures(baseProperty.Features, comparedProperty.Features),
                };

                result.OverallSimilarityScore = CalculateOverallSimilarityScore(result);
                result.AddressProximity = CompareAddressProximity();

                DetermineBetterProperty(baseProperty, comparedProperty, result);

                return Result<PropertyComparison>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<PropertyComparison>.Failure($"Error comparing properties: {ex.Message}");
            }
        }

        private TypeCompatibilityScore ComparePropertyTypes(PropertyType baseType, PropertyType comparedType)
        {
            var compatibilityMatrix = new Dictionary<(PropertyType, PropertyType), double>
            {
                { (PropertyType.Apartment, PropertyType.Studio), 0.8 },
                { (PropertyType.Studio, PropertyType.Apartment), 0.8 },
                { (PropertyType.House, PropertyType.Apartment), 0.5 },
                { (PropertyType.Office, PropertyType.CommercialSpace), 0.7 }
            };

            var key = (baseType, comparedType);
            var reverseKey = (comparedType, baseType);

            double score = compatibilityMatrix.TryGetValue(key, out double value)
                ? value
                : (compatibilityMatrix.ContainsKey(reverseKey) ? compatibilityMatrix[reverseKey] : (baseType == comparedType ? 1.0 : 0.3));

            return new TypeCompatibilityScore
            {
                BaseType = baseType,
                ComparedType = comparedType,
                Score = score
            };
        }

        private Dictionary<PropertyFeatureType, ComparisonDetail> CompareFeatures(
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

        private double CalculateFeatureSimilarity(PropertyFeatureType featureType, int baseValue, int comparedValue)
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

        private double CalculateOverallSimilarityScore(PropertyComparison result)
        {
            double typeScore = (result.TypeCompatibility?.Score ?? 0) * 0.2;
            double featureScore = CalculateFeatureScore(result.FeatureComparisons) * 0.5;
            double addressScore = (result.AddressProximity?.SimilarityScore ?? 0) * 0.3;

            return Math.Round(typeScore + featureScore + addressScore, 2);
        }

        private double CalculateFeatureScore(Dictionary<PropertyFeatureType, ComparisonDetail> featureComparisons)
        {
            var weights = new Dictionary<PropertyFeatureType, double>
            {
                { PropertyFeatureType.Rooms, 0.15 },
                { PropertyFeatureType.Surface, 0.15 },
                { PropertyFeatureType.Floor, 0.1 },
                { PropertyFeatureType.Year, 0.1 }
            };

            return featureComparisons.Sum(fc =>
            {
                double weight = weights.TryGetValue(fc.Key, out var w) ? w : 0.05;
                return fc.Value.SimilarityScore * weight;
            });
        }

        private void DetermineBetterProperty(Domain.Entities.Property baseProperty, Domain.Entities.Property comparedProperty, PropertyComparison result)
        {
            var baseScore = result.OverallSimilarityScore;
            var reasons = result.Reasons;

            if (baseScore > 0.5) // Threshold for better property
            {
                result.WinningProperty = baseProperty.Id.ToString();
                reasons.Add("Base property has better features overall.");
            }
            else
            {
                result.WinningProperty = comparedProperty.Id.ToString();
                reasons.Add("Compared property has superior features and similarity.");
            }
        }

        private AddressProximityAnalysis CompareAddressProximity()
        {
            return new AddressProximityAnalysis
            {
                DistanceInKilometers = 5.0,
                SimilarityScore = 0.9
            };
        }
    }
}
