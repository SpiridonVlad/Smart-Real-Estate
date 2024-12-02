using FluentValidation;

namespace Application.Use_Cases.Addresses
{
    public class GetAddressByIdQueryValidator : AbstractValidator<GetAddressByIdQuery>
    {
        public GetAddressByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                 .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Id must be a valid GUID.");
        }
    }
}
