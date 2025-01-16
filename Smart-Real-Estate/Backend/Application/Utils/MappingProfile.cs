using Domain.Entities;

using AutoMapper;
using Application.DTOs;
using Application.Use_Cases.Commands;

namespace Application.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Property, PropertyDto>().ReverseMap();
            CreateMap<CreatePropertyCommand, Property>().ReverseMap();
            CreateMap<UpdatePropertyCommand, Property>().ReverseMap();

            CreateMap<Listing, ListingDto>().ReverseMap();
            CreateMap<CreateListingCommand, Listing>().ReverseMap();
            CreateMap<UpdateListingCommand, Listing>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserCommand, User>().ReverseMap();
            CreateMap<UpdateUserCommand, User>().ReverseMap();

            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Record, RecordDto>().ReverseMap();
        }
    }
}
