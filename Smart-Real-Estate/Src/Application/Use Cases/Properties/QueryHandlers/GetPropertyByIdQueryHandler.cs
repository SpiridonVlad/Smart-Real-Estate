using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Property.QueryHandlers
{
    public class GetPropertyByIdQueryHandler(IMapper mapper,
        IPropertyRepository repository,
        IAddressRepository addressRepository
        ) : IRequestHandler<GetPropertyByIdQuery, Result<PropertyDto>>
    {
        private readonly IMapper mapper = mapper;
        private readonly IPropertyRepository repository = repository;
        private readonly IAddressRepository addressRepository = addressRepository;

        public async Task<Result<PropertyDto>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var propertyResult = await repository.GetByIdAsync(request.Id);
            if (propertyResult.IsSuccess)
            {
                var address = await addressRepository.GetByIdAsync(propertyResult.Data.AddressId);
                if (!address.IsSuccess)
                {
                    return Result<PropertyDto>.Failure("Address not found.");
                }
                var addressDto = mapper.Map<AddressDto>(address.Data);
                propertyResult.Data.Address = mapper.Map<Address>(addressDto);
                return Result<PropertyDto>.Success(mapper.Map<PropertyDto>(propertyResult.Data));
            }
            return Result<PropertyDto>.Failure(propertyResult.ErrorMessage);
        }
    }
}
