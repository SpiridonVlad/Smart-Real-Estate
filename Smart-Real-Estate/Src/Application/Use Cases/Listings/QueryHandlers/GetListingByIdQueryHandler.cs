using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetListingByIdQueryHandler: IRequestHandler<GetListingByIdQuery, Result<ListingDto>>
    {
        private readonly IListingRepository repository;
        private readonly IMapper mapper;

        public GetListingByIdQueryHandler(IListingRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

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
