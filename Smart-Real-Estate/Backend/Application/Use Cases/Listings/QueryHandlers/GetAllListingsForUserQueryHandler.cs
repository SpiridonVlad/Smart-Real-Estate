using Application.DTOs;
using Application.Use_Cases.Listings.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Listings.QueryHandlers
{
    public class GetAllListingsForUserQueryHandler(IListingRepository listingRepository, IMapper mapper) : IRequestHandler<GetAllListingsForUserQuery, Result<IEnumerable<ListingDto>>>
    {
        private readonly IListingRepository listingRepository = listingRepository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<IEnumerable<ListingDto>>> Handle(GetAllListingsForUserQuery request, CancellationToken cancellationToken)
        {
            var listingsResult = await listingRepository.GetAllForUserAsync(request.UserId);
            if (!listingsResult.IsSuccess)
            {
                return Result<IEnumerable<ListingDto>>.Failure("Failed to retrieve listings.");
            }
            return Result<IEnumerable<ListingDto>>.Success(mapper.Map<IEnumerable<ListingDto>>(listingsResult.Data));
        }
    }
}
