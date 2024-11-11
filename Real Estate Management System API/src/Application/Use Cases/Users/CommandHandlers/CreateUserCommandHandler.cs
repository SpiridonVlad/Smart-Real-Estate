using Application.Use_Cases.Commands;
using Application.Use_Cases.Users.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IPropertyRepository propertyRepository;

        public CreateUserCommandHandler(IUserRepository repository, IMapper mapper, IPropertyRepository propertyRepository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            CreateUserCommandValidator validator = new CreateUserCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<Guid>.Failure(validationResult.ToString());
            }
            var user = mapper.Map<User>(request);
            // Handle null PropertyHistory
            if (user.PropertyHistory == null)
            {
                user.PropertyHistory = new List<Guid>();
            }
            foreach(var propertyId in user.PropertyHistory)
            {
                var propertyResult = await propertyRepository.GetByIdAsync(propertyId);
                if (!propertyResult.IsSuccess)
                {
                    return Result<Guid>.Failure("Property not found");
                }
            }
            var result = await repository.AddAsync(user);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}
