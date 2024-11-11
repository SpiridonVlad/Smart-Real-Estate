using Application.Use_Cases.Commands;
using Domain.Types;
using FluentValidation;

public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator()
    {
        // Validate that Id is not empty
        RuleFor(x => x.Id).NotEmpty().WithMessage("Property ID must not be empty.");

        // Validate that AddressId is not empty
        RuleFor(x => x.AddressId).NotEmpty().WithMessage("Address ID must not be empty.");

        // Validate that Address is not null and its Id is not empty
        RuleFor(x => x.Address.Id).NotEmpty().WithMessage("Address ID must not be empty.");
        RuleFor(x => x.Address.Street)
            .NotEmpty().WithMessage("Street must not be empty.")
            .MaximumLength(100).WithMessage("Street must not exceed 100 characters.");

        // Validate that City is not empty and does not exceed 50 characters
        RuleFor(x => x.Address.City)
            .NotEmpty().WithMessage("City must not be empty.")
            .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

        // Validate that State is not empty and does not exceed 50 characters
        RuleFor(x => x.Address.State)
            .NotEmpty().WithMessage("State must not be empty.")
            .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

        // Validate that PostalCode is not empty and matches the Romanian format
        RuleFor(x => x.Address.PostalCode)
            .NotEmpty().WithMessage("Postal code must not be empty.")
            .Matches(@"^\d{6}$").WithMessage("Postal code must be a 6-digit number.");

        // Validate that Country is not empty and does not exceed 50 characters
        RuleFor(x => x.Address.Country)
            .NotEmpty().WithMessage("Country must not be empty.")
            .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

        // Validate that ImageId is not empty and does not exceed 50 characters
        RuleFor(x => x.ImageId)
            .NotEmpty().WithMessage("Image ID must not be empty.")
            .MaximumLength(50).WithMessage("Image ID must not exceed 50 characters.");

        // Validate that UserId is not empty
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID must not be empty.");

        // Validate that Type is a valid enum value
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid property type.");

        // Validate that Features is not null
        RuleFor(x => x.Features).NotNull().WithMessage("Features must not be null.");

        // Validate that each feature key is a valid enum value
        RuleForEach(x => x.Features.Features)
            .Must(feature => Enum.IsDefined(typeof(PropertyFeatureType), feature.Key))
            .WithMessage("Invalid property feature type.");
    }
}