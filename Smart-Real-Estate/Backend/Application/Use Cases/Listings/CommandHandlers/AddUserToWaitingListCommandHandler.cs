using Application.Use_Cases.Listings.Commands;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Properties.CommandHandlers
{
    public class AddUserToWaitingListCommandHandler(IListingRepository listingRepository, IUserRepository userRepository) : IRequestHandler<AddUserToWaitingListCommand, Result<string>>
    {
        private readonly IListingRepository listingRepository = listingRepository;
        private readonly IUserRepository userRepository = userRepository;

        public async Task<Result<string>> Handle(AddUserToWaitingListCommand request, CancellationToken cancellationToken)
        {
            var listingExists = await listingRepository.GetByIdAsync(request.ListingId);
            if (!listingExists.IsSuccess)
            {
                return Result<string>.Failure("ListingId does not exist.");
            }
            var userExists = await userRepository.GetByIdAsync(request.UserId);
            if (!userExists.IsSuccess)
            {
                return Result<string>.Failure("UserId does not exist.");
            }
            listingExists.Data.UserWaitingList.Add(request.UserId);
            var result = await listingRepository.UpdateAsync(listingExists.Data);
            if (result.IsSuccess)
            {
                return Result<string>.Success("User added to waiting list.");
            }
            return Result<string>.Failure($"Failed to add user to waiting list: {result.ErrorMessage}");
        }
    }
}
