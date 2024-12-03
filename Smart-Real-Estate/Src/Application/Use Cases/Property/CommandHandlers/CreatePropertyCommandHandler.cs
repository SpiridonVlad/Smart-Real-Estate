using Application.Use_Cases.Commands;
using Application.Use_Cases.Property.Commands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Property.CommandHandlers
{

    public class CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IAddressRepository addressRepository,
        IMapper mapper,
        IUserRepository userRepository) : IRequestHandler<CreatePropertyCommand, Result<Guid>>
    {
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IAddressRepository addressRepository = addressRepository;
        private readonly IMapper mapper = mapper;
        private readonly IUserRepository userRepository = userRepository;

        public async Task<Result<Guid>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            CreatePropertyCommandValidator validator = new();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<Guid>.Failure(validationResult.ToString());
            }

            var userExists = await userRepository.GetByIdAsync(request.UserId);
            if (!userExists.IsSuccess)
            {
                return Result<Guid>.Failure("UserId does not exist.");
            }

            var addressResult = await addressRepository.AddAsync(request.Address);
            if (!addressResult.IsSuccess)
            {
                return Result<Guid>.Failure($"Failed to create address: {addressResult.ErrorMessage}");
            }

            var property = mapper.Map<Domain.Entities.Property>(request);
            property.AddressId = addressResult.Data;

            var propertyResult = await propertyRepository.AddAsync(property);
            if (propertyResult.IsSuccess)
            {
                return Result<Guid>.Success(propertyResult.Data);
            }

            return Result<Guid>.Failure($"Failed to create property: {propertyResult.ErrorMessage}");
        }
    }
}
