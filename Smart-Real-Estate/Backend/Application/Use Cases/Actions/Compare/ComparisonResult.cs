using Domain.Common;
using Domain.Comparison;
using MediatR;

namespace Application.Use_Cases.Actions
{
    public class ComparisonResult
    {
        public bool? IsSuccessful { get; set; }
        public string? Message { get; set; }
        public PropertyComparison? ComparisonDetails { get; set; }
        public List<string>? Recommendations { get; set; }
        public ComparisonSeverity? Severity { get; set; }

        public enum ComparisonSeverity
        {
            Low,
            Medium,
            High,
            Critical
        }

        public void GenerateRecommendations()
        {
            Recommendations = [];

            if (ComparisonDetails == null)
                return;

            if (ComparisonDetails.TypeCompatibility.Score < 0.5)
            {
                Recommendations.Add($"Property types are quite different: {ComparisonDetails.TypeCompatibility.BaseType} vs {ComparisonDetails.TypeCompatibility.ComparedType}");
                Severity = ComparisonSeverity.High;
            }

            foreach (var feature in ComparisonDetails.FeatureComparisons)
            {
                if (feature.Value.SimilarityScore < 0.3)
                {
                    Recommendations.Add($"Significant difference in {feature.Key}: Base value {feature.Value.BaseValue} vs Compared value {feature.Value.ComparedValue}");
                    Severity = ComparisonSeverity.Critical;
                }
            }

            if (ComparisonDetails.OverallSimilarityScore < 0.4)
            {
                Recommendations.Add("Properties have low overall similarity");
                Severity = ComparisonSeverity.Critical;
            }

            if (Recommendations.Count == 0)
            {
                Recommendations.Add("Properties are quite similar");
                Severity = ComparisonSeverity.Low;
            }
        }
    }
}
