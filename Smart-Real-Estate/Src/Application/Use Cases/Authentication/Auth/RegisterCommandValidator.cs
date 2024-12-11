using FluentValidation;

namespace Application.Authentication
{
    public class RegisterCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must be less than 100 characters")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Invalid email format");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Username must be less than 50 characters long.")
                .Matches(@"^[a-zA-Z0-9]([._-]?[a-zA-Z0-9]+)*$")
                    .WithMessage("Username must start and end with an alphanumeric character and can only contain letters, numbers, underscores, hyphens, or periods. Special characters cannot be consecutive.");

            RuleFor(x => x.Type).IsInEnum().WithMessage("Invalid user type");
        }
    }
}
