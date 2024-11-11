using Application.Use_Cases.Commands;
using FluentValidation;

public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AddressId).NotEmpty();
        RuleFor(x => x.Address.Id).NotEmpty();
        RuleFor(x => x.Address.Street).NotEmpty();
        RuleFor(x => x.Address.City).NotEmpty();
        RuleFor(x => x.Address.State).NotEmpty();
        RuleFor(x => x.Address.PostalCode).NotEmpty();
        RuleFor(x => x.Address.Country).NotEmpty();
        RuleFor(x => x.ImageId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Features).NotNull();
        // Add other validation rules as needed
    }
}
