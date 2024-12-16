using Application.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Cards;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Wrappers
{
    public class GetRecordByIdQueryHandler(IUserRepository userRepository,
     IPropertyRepository propertyRepository,
     IAddressRepository addressRepository,
     IListingRepository listingRepository,
     IMapper mapper) : IRequestHandler<GetRecordByIdQuery, Result<RecordDto>>
    {
        private readonly IUserRepository userRepository = userRepository;
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IAddressRepository addressRepository = addressRepository;
        private readonly IListingRepository listingRepository = listingRepository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<RecordDto>> Handle(GetRecordByIdQuery request, CancellationToken cancellationToken)
        {
            var listing = await listingRepository.GetByIdAsync(request.Id);
            if (!listing.IsSuccess)
            {
                return Result<RecordDto>.Failure("Failed to retrieve listings.");
            }

            var user = await userRepository.GetByIdAsync(listing.Data.UserId);
            if (!user.IsSuccess)
            {
                return Result<RecordDto>.Failure("User not found.");
            }

            var property = await propertyRepository.GetByIdAsync(listing.Data.PropertyId);
            if (!property.IsSuccess)
            {
                return Result<RecordDto>.Failure("Property not found.");
            }

            var address = await addressRepository.GetByIdAsync(property.Data.AddressId);
            if (!address.IsSuccess)
            {
                return Result<RecordDto>.Failure("Address not found.");
            }

            var record = new Record
            {
                Address = new Address
                {
                    Street = address.Data.Street,
                    City = address.Data.City,
                    State = address.Data.State,
                    PostalCode = address.Data.PostalCode,
                    Country = address.Data.Country,
                    AdditionalInfo = address.Data.AdditionalInfo
                },

                Property = new PropertyCard
                {
                    ImageId = property.Data.ImageId,
                    Type = property.Data.Type,
                    Features = property.Data.Features
                },

                User = new UserCard
                {
                    Username = user.Data.Username,
                    Verified = user.Data.Verified,
                    Rating = user.Data.Rating,
                    Type = user.Data.Type
                },

                Listing = new ListingCard
                {
                    Description = listing.Data.Description,
                    Price = listing.Data.Price,
                    PublicationDate = listing.Data.PublicationDate,
                    Features = listing.Data.Features
                }
            };

            return Result<RecordDto>.Success(mapper.Map<RecordDto>(record));
        }
    }
}
