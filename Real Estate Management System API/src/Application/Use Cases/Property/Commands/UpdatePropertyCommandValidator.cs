using Application.Use_Cases.Commands;
using FluentValidation;

public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Surface).GreaterThan(0);
        RuleFor(x => x.Rooms).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty();
        // Add other validation rules as needed
    }
}
