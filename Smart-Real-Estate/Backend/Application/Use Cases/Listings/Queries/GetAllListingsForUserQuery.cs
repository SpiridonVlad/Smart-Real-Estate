using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Listings.Queries
{
    public class GetAllListingsForUserQuery: IRequest<Result<IEnumerable<ListingDto>>>
    {
        public Guid UserId { get; set; }
    }
}
