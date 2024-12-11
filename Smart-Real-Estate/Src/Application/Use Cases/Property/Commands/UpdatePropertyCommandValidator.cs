using Domain.Types;
using FluentValidation;

namespace Application.Use_Cases.Commands
{
    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Property ID must not be empty.");

            RuleFor(x => x.AddressId).NotEmpty().WithMessage("Address ID must not be empty.");

            RuleFor(x => x.Address.Id).NotEmpty().WithMessage("Address ID must not be empty.");

            RuleFor(x => x.Address.Street)
                .NotEmpty().WithMessage("Street must not be empty.")
                .MaximumLength(100).WithMessage("Street must not exceed 100 characters.");

            RuleFor(x => x.Address.City)
                .NotEmpty().WithMessage("City must not be empty.")
                .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

            RuleFor(x => x.Address.State)
                .NotEmpty().WithMessage("State must not be empty.")
                .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

            RuleFor(x => x.Address.PostalCode)
                .NotEmpty().WithMessage("Postal code must not be empty.")
                .Matches(@"^\d{6}$").WithMessage("Postal code must be a 6-digit number.");

            RuleFor(x => x.Address.Country)
                .NotEmpty().WithMessage("Country must not be empty.")
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

            RuleFor(x => x.ImageId)
                .NotEmpty().WithMessage("Image ID must not be empty.")
                .MaximumLength(50).WithMessage("Image ID must not exceed 50 characters.");

            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID must not be empty.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid property type.");

            RuleFor(x => x.Features).NotNull().WithMessage("Features must not be null.");

            RuleForEach(x => x.Features.Features)
                .Must(feature => Enum.IsDefined(feature.Key))
                .WithMessage("Invalid property feature type.");
        }
    }
}