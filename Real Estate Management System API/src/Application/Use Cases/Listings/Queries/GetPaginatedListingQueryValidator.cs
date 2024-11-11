using Application.Use_Cases.Queries;
using FluentValidation;

namespace Application.Use_Cases.Listings.Queries
{
    class GetPaginatedListingQueryValidator : AbstractValidator<GetPaginatedListingsQuery>
    {
        public GetPaginatedListingQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
