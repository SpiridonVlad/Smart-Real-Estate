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
                .NotEmpty().WithMessage("Listing Id is required for update.");

            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("PropertyId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.PublicationDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Publication date cannot be in the future.")
                .When(x => x.PublicationDate.HasValue);

            RuleFor(x => x.Properties)
                .Must(p => p == null || p.TrueForAll(item => Enum.IsDefined(typeof(ListingAssetss), item)))
                .WithMessage("Properties list contains invalid values.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(x => x.Description != null);
        }
    }
}
