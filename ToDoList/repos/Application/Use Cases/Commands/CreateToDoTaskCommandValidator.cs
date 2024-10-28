using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.Commands
{
    public class CreateToDoTaskCommandValidator : AbstractValidator<CreateToDoTaskCommand>
    {
        public CreateToDoTaskCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200).WithMessage("'{ProperTyName}' must not be null or over 200 characters!");
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.DueDate).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
