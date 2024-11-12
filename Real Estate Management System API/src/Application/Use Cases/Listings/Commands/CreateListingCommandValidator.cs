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
                .NotEmpty().WithMessage("PropertyId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.PublicationDate)
                .NotEmpty().WithMessage("Publication date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Publication date cannot be in the future.");

            RuleFor(x => x.Properties)     
                .NotNull().WithMessage("Properties list is required.")
                .Must(p => p.All(item => Enum.IsDefined(typeof(ListingAssetss), item)))
                .WithMessage("Properties list contains invalid values.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
