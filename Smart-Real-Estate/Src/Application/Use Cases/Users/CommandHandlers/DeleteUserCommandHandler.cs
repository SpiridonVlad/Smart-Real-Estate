using MediatR;
using Domain.Repositories;
using Domain.Common;
using Application.Use_Cases.Users.Commands;

namespace Application.Use_Cases.Users.CommandHandlers
{
    public class DeleteUserCommandHandler(IUserRepository repository,IPropertyRepository propertyRepository, IListingRepository listingRepository) : IRequestHandler<DeleteUserCommand, Result<string>>
    {
        private readonly IUserRepository repository = repository;
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IListingRepository listingRepository = listingRepository;

        public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userResult = await repository.DeleteAsync(request.Id);
            var listingResult = await listingRepository.DeleteUsersListings(request.Id);
            var propertyResult = await propertyRepository.DeleteUsersPropertys(request.Id);
            if (userResult.IsSuccess && listingResult.IsSuccess && propertyResult.IsSuccess)
            {
                return Result<string>.Success("User deleted successfully");
            }
            else
            {
                return Result<string>.Failure("User deletion failed");
            }

        }
    }
}