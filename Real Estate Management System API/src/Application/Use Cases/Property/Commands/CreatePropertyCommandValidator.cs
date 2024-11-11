using FluentValidation;
using Application.Use_Cases.Commands;

namespace Application.Use_Cases.Property.Commands
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Address.Street)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

            RuleFor(x => x.Address.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.Address.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(100).WithMessage("State cannot exceed 100 characters.");

            RuleFor(x => x.Address.PostalCode)
                .NotEmpty().WithMessage("PostalCode is required.")
                .MaximumLength(100).WithMessage("PostalCode cannot exceed 100 characters.");

            RuleFor(x => x.Address.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");

            RuleFor(x => x.ImageId)
                .NotEmpty().WithMessage("ImageId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid property type.");
        }
    }
}
