using Application.Use_Cases.Commands;
using Domain.Types;
using FluentValidation;

namespace Application.Use_Cases.Listings.Commands
{
    public class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
    {
        public CreateListingCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("PropertyId is required.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("PropertyId must be a valid GUID.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.PublicationDate)
                .NotEmpty().WithMessage("Publication date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Publication date cannot be in the future.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            //RuleFor(x => x.Features)
            //    .NotNull().WithMessage("Features are required.")
            //    .Must(features => features.Features != null && features.Features.Count > 0)
            //    .WithMessage("Features must include at least one feature.")
            //    .DependentRules(() =>
            //    {
            //        RuleForEach(x => x.Features.Features)
            //            .Must(kvp => Enum.IsDefined(typeof(ListingAssetss), kvp.Key))
            //            .WithMessage("Invalid feature type.")
            //            .Must(kvp => kvp.Value >= 0)
            //            .WithMessage("Feature values must be zero or positive.");
            //    });
        }
    }
}
