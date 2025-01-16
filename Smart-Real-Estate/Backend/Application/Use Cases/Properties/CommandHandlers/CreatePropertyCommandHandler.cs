using Application.Use_Cases.Commands;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Use_Cases.Property.CommandHandlers
{

    public class CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IAddressRepository addressRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
    IUserRepository userRepository) : IRequestHandler<CreatePropertyCommand, Result<Guid>>
    {
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IAddressRepository addressRepository = addressRepository;
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IMapper mapper = mapper;

        public async Task<Result<Guid>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var userIdResult = JwtHelper.GetUserIdFromJwt(httpContextAccessor.HttpContext);

            var addressResult = await addressRepository.AddAsync(request.Address);
            if (!addressResult.IsSuccess)
            {
                return Result<Guid>.Failure($"Failed to create address: {addressResult.ErrorMessage}");
            }

            var property = mapper.Map<Domain.Entities.Property>(request);
            property.UserId = userIdResult.Data;
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
