using Application.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Cards;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Wrappers
{
    public class GetRealtyQuerryHandler(IPropertyRepository propertyRepository,
     IAddressRepository addressRepository,
     IMapper mapper) : IRequestHandler<GetRealtyQuery, Result<IEnumerable<RealtyDto>>>
    {
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IAddressRepository addressRepository = addressRepository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<IEnumerable<RealtyDto>>> Handle(GetRealtyQuery request, CancellationToken cancellationToken)
        {
            var propertyResult = await propertyRepository.GetAllForUser(request.Id);
            if (!propertyResult.IsSuccess)
            {
                return Result<IEnumerable<RealtyDto>>.Failure("Failed to retrieve properties.");
            }

            var Realtys = new List<Realty>();
            foreach (var property in propertyResult.Data)
            {
                var address = await addressRepository.GetByIdAsync(property.AddressId);
                if (!address.IsSuccess)
                {
                    return Result<IEnumerable<RealtyDto>>.Failure("Address not found.");
                }

                var record = new Realty
                {
                    Property = new PropertyCard
                    {
                        ImageId = property.ImageId,
                        PType = property.Type,
                        PFeatures = property.Features,
                    },
                    Address = new Address
                    {
                        City = address.Data.City,
                        Country = address.Data.Country,
                        PostalCode = address.Data.PostalCode,
                        Street = address.Data.Street,
                        State = address.Data.State,
                    }
                };

                Realtys.Add(record);
            }

            return Result<IEnumerable<RealtyDto>>.Success(mapper.Map<IEnumerable<RealtyDto>>(Realtys));

        }
    }
}
