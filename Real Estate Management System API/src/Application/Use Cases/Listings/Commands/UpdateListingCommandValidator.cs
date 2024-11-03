using Application.Use_Cases.Commands;
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

            RuleFor(x => x.IsSold)
                .NotNull().WithMessage("IsSold status is required.")
                .When(x => x.IsSold.HasValue); 

            RuleFor(x => x.IsHighlighted)
                .NotNull().WithMessage("IsHighlighted status is required.")
                .When(x => x.IsHighlighted.HasValue); 

            RuleFor(x => x.IsDeleted)
                .NotNull().WithMessage("IsDeleted status is required.")
                .When(x => x.IsDeleted.HasValue); 
        }
    }
}
