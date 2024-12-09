using FluentValidation;

namespace Application.Authentication
{
    public class LoginCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must be less than 100 characters")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Invalid email format"); 
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
