using Application.Use_Cases.Users.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Users.CommandHandlers
{
    public class AddPropertyToHistoryCommandHandler : IRequestHandler<AddPropertyToHistoryCommand, Result<string>>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IPropertyRepository propertyRepository;

        public AddPropertyToHistoryCommandHandler(IUserRepository repository, IMapper mapper, IPropertyRepository propertyRepository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
        }
        public async Task<Result<string>> Handle(AddPropertyToHistoryCommand request, CancellationToken cancellationToken)
        {
            // Check if user exists
            var userResult = await repository.GetByIdAsync(request.UserId);
            if (!userResult.IsSuccess)
            {
                return Result<string>.Failure("User not found");
            }

            // Check if property exists
            var propertyResult = await propertyRepository.GetByIdAsync(request.PropertyId);
            if (!propertyResult.IsSuccess)
            {
                return Result<string>.Failure("Property not found");
            }

            // Add property to user's history
            var user = userResult.Data;
            if (user.PropertyHistory == null)
            {
                user.PropertyHistory = new List<Guid>();
            }
            user.PropertyHistory.Add(request.PropertyId);

            // Update user
            var updateResult = await repository.UpdateAsync(user);
            if (updateResult.IsSuccess)
            {
                return Result<string>.Success("Property added to user's history successfully");
            }

            return Result<string>.Failure(updateResult.ErrorMessage);

        }
    }
}
