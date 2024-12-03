using Application.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Addresses
{
    public class GetAddressByIdQueryHandler(IAddressRepository addressRepository,IMapper mapper) : IRequestHandler<GetAddressByIdQuery, Result<AddressDto>>
    {
        private readonly IAddressRepository _addressRepository = addressRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<Result<AddressDto>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository.GetByIdAsync(request.Id);
            
            if (address.IsSuccess)
            {
                return Result<AddressDto>.Success(_mapper.Map<AddressDto>(address.Data));
            }
            return Result<AddressDto>.Failure(address.ErrorMessage);
        }
    }
}
