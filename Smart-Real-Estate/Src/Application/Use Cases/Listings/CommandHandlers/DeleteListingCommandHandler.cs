using Application.Use_Cases.Listings.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Listings.CommandHandlers
{
    public class DeleteListingCommandHandler : IRequestHandler<DeleteListingCommand, Result<string>>
    {
        private readonly IListingRepository repository;

        public DeleteListingCommandHandler(IListingRepository repository, IMapper mapper)
        {
            this.repository = repository;
        }


        public async Task<Result<string>> Handle(DeleteListingCommand request, CancellationToken cancellationToken)
        {
            DeleteListingCommandValidator validator = new DeleteListingCommandValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result<string>.Failure(validationResult.ToString());
            }

            var listing = await repository.DeleteAsync(request.Id);
            if (listing.IsSuccess)
            {
                return Result<string>.Success("Listing deleted successfully");
            }
            else
            {
                return Result<string>.Failure(listing.ErrorMessage);
            }
        }

    }
}
