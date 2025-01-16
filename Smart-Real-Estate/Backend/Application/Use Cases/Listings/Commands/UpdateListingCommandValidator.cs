using Application.Use_Cases.Commands;
using Domain.Types;
using FluentValidation;

namespace Application.Use_Cases.Listings.Commands
{
    public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
    {
        public UpdateListingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Listing Id is required for update.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Id must be a valid GUID.");

            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("PropertyId is required.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("PropertyId must be a valid GUID.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("UserId must be a valid GUID.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.PublicationDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Publication date cannot be in the future.")
                .When(x => x.PublicationDate.HasValue);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(x => x.Description != null);

            //RuleFor(x => x.Features)
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
