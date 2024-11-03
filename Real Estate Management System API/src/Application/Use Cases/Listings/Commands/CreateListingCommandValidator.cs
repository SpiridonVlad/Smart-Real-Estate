using Application.Use_Cases.Commands;
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

            RuleFor(x => x.IsSold)
                .NotNull().WithMessage("IsSold status is required.");

            RuleFor(x => x.IsHighlighted)
                .NotNull().WithMessage("IsHighlighted status is required.");

            RuleFor(x => x.IsDeleted)
                .NotNull().WithMessage("IsDeleted status is required.");
        }
    }
}
