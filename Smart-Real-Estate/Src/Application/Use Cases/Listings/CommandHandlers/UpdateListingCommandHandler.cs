using MediatR;
using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Repositories;
using Domain.Entities;
using Domain.Common;
using Application.Use_Cases.Listings.Commands;

namespace Application.Use_Cases.CommandHandlers
{
    public class UpdateListingCommandHandler(IListingRepository repository, IMapper mapper) : IRequestHandler<UpdateListingCommand, Result<string>>
    {
        private readonly IListingRepository repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<string>> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
        {
            var listing = mapper.Map<Listing>(request);
            var result = await repository.UpdateAsync(listing);
            if (result.IsSuccess)
            {
                return Result<string>.Success("Listing updated successfully");
            }
            return Result<string>.Failure(result.ErrorMessage);
        }
    }
}
