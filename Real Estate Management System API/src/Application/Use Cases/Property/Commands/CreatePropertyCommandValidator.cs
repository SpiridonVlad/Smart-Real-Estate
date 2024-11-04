using FluentValidation;
using Application.Use_Cases.Commands;

namespace Application.Use_Cases.Property.Commands
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

            RuleFor(x => x.Surface)
                .GreaterThan(0).WithMessage("Surface must be greater than 0.");

            RuleFor(x => x.Rooms)
                .GreaterThan(0).WithMessage("Rooms must be greater than 0.");

            RuleFor(x => x.ImageId)
                .NotEmpty().WithMessage("ImageId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid property type.");

            RuleFor(x => x.HasGarden)
                .NotNull().WithMessage("HasGarden status is required.");

            RuleFor(x => x.HasGarage)
                .NotNull().WithMessage("HasGarage status is required.");

            RuleFor(x => x.HasPool)
                .NotNull().WithMessage("HasPool status is required.");

            RuleFor(x => x.HasBalcony)
                .NotNull().WithMessage("HasBalcony status is required.");
        }
    }
}
