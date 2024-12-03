using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetListingByIdQueryHandler(IListingRepository repository, IMapper mapper) : IRequestHandler<GetListingByIdQuery, Result<ListingDto>>
    {
        private readonly IListingRepository repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<ListingDto>> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
        {
            var listingResult = await repository.GetByIdAsync(request.Id);
            if(listingResult.IsSuccess)
            {
                return Result<ListingDto>.Success(mapper.Map<ListingDto>(listingResult.Data));
            }
            return Result<ListingDto>.Failure(listingResult.ErrorMessage);
        }
    }
}
