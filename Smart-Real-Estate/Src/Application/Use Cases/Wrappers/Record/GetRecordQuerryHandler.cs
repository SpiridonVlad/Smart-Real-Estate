using Application.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Cards;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Wrappers
{
    public class GetRecordQuerryHandler(IUserRepository userRepository,
     IPropertyRepository propertyRepository,
     IAddressRepository addressRepository,
     IListingRepository listingRepository,
     IMapper mapper) : IRequestHandler<GetRecordQuery, Result<IEnumerable<RecordDto>>>
    {
        private readonly IUserRepository userRepository = userRepository;
        private readonly IPropertyRepository propertyRepository = propertyRepository;
        private readonly IAddressRepository addressRepository = addressRepository;
        private readonly IListingRepository listingRepository = listingRepository;
        private readonly IMapper mapper = mapper;

        public async Task<Result<IEnumerable<RecordDto>>> Handle(GetRecordQuery request, CancellationToken cancellationToken)
        {
            var listingsResult = await listingRepository.GetPaginatedAsync(request.Page, request.PageSize);
            if (!listingsResult.IsSuccess)
            {
                return Result<IEnumerable<RecordDto>>.Failure("Failed to retrieve listings.");
            }

            var Records = new List<Record>();
            foreach (var listing in listingsResult.Data)
            {
                var user = await userRepository.GetByIdAsync(listing.UserId);
                if (!user.IsSuccess)
                {
                    return Result<IEnumerable<RecordDto>>.Failure("User not found.");
                }

                var property = await propertyRepository.GetByIdAsync(listing.PropertyId);
                if (!property.IsSuccess)
                {
                    return Result<IEnumerable<RecordDto>>.Failure("Property not found.");
                }

                var address = await addressRepository.GetByIdAsync(property.Data.AddressId);
                if (!address.IsSuccess)
                {
                    return Result<IEnumerable<RecordDto>>.Failure("Address not found.");
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
                        PType = property.Data.Type,
                        PFeatures = property.Data.Features
                    },

                    User = new UserCard
                    {
                        Username = user.Data.Username,
                        Verified = user.Data.Verified,
                        Rating = user.Data.Rating,
                        UType = user.Data.Type
                    },

                    Listing = new ListingCard
                    {
                        Description = listing.Description,
                        Price = listing.Price,
                        PublicationDate = listing.PublicationDate,
                        LFeatures = listing.Features
                    }
                };

                Records.Add(record);
            }

            return Result<IEnumerable<RecordDto>>.Success(mapper.Map<IEnumerable<RecordDto>>(Records));
            
        }
    }
}
