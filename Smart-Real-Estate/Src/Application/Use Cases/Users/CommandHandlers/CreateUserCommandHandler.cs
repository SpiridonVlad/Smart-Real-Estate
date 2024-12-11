using Application.Use_Cases.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers
{
    public class CreateUserCommandHandler(IUserRepository repository, IMapper mapper, IPropertyRepository propertyRepository) : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUserRepository repository = repository;
        private readonly IMapper mapper = mapper;
        private readonly IPropertyRepository propertyRepository = propertyRepository;

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(request);

            user.PropertyHistory ??= [];

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
