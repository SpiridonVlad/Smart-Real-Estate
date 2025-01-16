using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetPaginatedListingsQueryHandler(IListingRepository repository, IMapper mapper) : IRequestHandler<GetPaginatedListingsQuery, Result<IEnumerable<ListingDto>>>
    {
        private readonly IListingRepository repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<IEnumerable<ListingDto>>> Handle(GetPaginatedListingsQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter.BuildFilterExpression();
            var listings = await repository.GetPaginatedAsync(request.Page, request.PageSize,filter);
            if (listings.IsSuccess)
            {
                return Result<IEnumerable<ListingDto>>.Success(mapper.Map<IEnumerable<ListingDto>>(listings.Data));
            }
            return Result<IEnumerable<ListingDto>>.Failure(listings.ErrorMessage);
        }
    }
}
