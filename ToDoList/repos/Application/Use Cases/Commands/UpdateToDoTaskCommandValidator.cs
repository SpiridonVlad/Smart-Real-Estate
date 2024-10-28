using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.Commands
{
    public class UpdateToDoTaskCommandValidator:  AbstractValidator<UpdateToDoTaskCommand>
    {
        public UpdateToDoTaskCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().Must(BeAValidGuid).WithMessage("'{ProperTyName}' must be a valid Guid.");
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200).WithMessage("'{ProperTyName}' must not be null or over 200 characters!");
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.DueDate).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
