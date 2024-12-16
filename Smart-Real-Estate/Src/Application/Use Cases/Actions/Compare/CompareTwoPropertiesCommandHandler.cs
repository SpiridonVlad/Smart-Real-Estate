using Domain.Common;
using Domain.Comparison;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Actions
{
    public class CompareTwoPropertiesCommandHandler(IPropertyRepository propertyRepository) : IRequestHandler<CompareTwoPropertiesCommand, Result<ComparisonResult>>
    {
        private readonly IPropertyRepository propertyRepository = propertyRepository;

        public async Task<Result<ComparisonResult>> Handle(CompareTwoPropertiesCommand request, CancellationToken cancellationToken)
        {
            var result = new ComparisonResult
            {
                IsSuccessful = false
            };

            try
            {
                var baseProperty = await propertyRepository.GetByIdAsync(request.Initial);
                var comparedProperty = await propertyRepository.GetByIdAsync(request.Secondary);

                if(!baseProperty.IsSuccess && !comparedProperty.IsSuccess)
                {
                    result.Message = "Properties not found";
                    return Result<ComparisonResult>.Failure(result.Message);
                }

                var comparison = PropertyComparison.Compare(baseProperty.Data, comparedProperty.Data);
                result.ComparisonDetails = comparison;

                result.GenerateRecommendations();

                result.IsSuccessful = true;
                result.Message = "Properties successfully compared";

                return Result<ComparisonResult>.Success(result);
            }
            catch (Exception ex)
            {
                result.Message = $"Error during property comparison: {ex.Message}";
                return Result<ComparisonResult>.Failure(result.Message);
            }
        }
    }
}
