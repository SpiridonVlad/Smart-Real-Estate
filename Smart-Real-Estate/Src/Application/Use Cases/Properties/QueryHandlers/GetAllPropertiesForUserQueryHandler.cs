using Application.DTOs;
using Application.Use_Cases.Property.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Property.QueryHandlers
{
    public class GetAllPropertiesForUserQueryHandler(IMapper mapper, IPropertyRepository propertyRepository,IAddressRepository addressRepository) : IRequestHandler<GetAllPropertiesForUserQuery, Result<IEnumerable<PropertyDto>>>
    {
        private readonly IMapper mapper = mapper;
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IAddressRepository addressRepository = addressRepository;
        public async Task<Result<IEnumerable<PropertyDto>>> Handle(GetAllPropertiesForUserQuery request, CancellationToken cancellationToken)
        {
            var properties = await propertyRepository.GetAllForUserAsync(request.UserId);

            List<PropertyDto> propertyDtos = [];

            if (properties.IsSuccess)
            {
                foreach (var property in properties.Data)
                {
                    var address = await addressRepository.GetByIdAsync(property.AddressId);
                    if (!address.IsSuccess)
                    {
                        return Result<IEnumerable<PropertyDto>>.Failure("Address not found.");
                    }
                    property.Address = address.Data;
                    propertyDtos.Add(mapper.Map<PropertyDto>(property));
                }
                return Result<IEnumerable<PropertyDto>>.Success(mapper.Map<IEnumerable<PropertyDto>>(propertyDtos));
            }
            return Result<IEnumerable<PropertyDto>>.Failure("Properties not found");
        }
    }
}
