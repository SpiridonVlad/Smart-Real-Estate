using FluentValidation;

namespace Application.Use_Cases.Listings.Commands
{
    public class DeleteListingCommandValidator : AbstractValidator<DeleteListingCommand>
    {
        public DeleteListingCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
