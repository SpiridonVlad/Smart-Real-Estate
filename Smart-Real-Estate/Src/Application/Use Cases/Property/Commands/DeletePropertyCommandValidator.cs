using FluentValidation;

namespace Application.Use_Cases.Property.Commands
{
    public class DeletePropertyCommandValidator : AbstractValidator<DeletePropertyCommand>
    {
        public DeletePropertyCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
