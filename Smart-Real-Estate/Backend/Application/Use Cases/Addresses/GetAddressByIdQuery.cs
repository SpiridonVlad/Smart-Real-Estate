using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Addresses
{
    public class GetAddressByIdQuery : IRequest<Result<AddressDto>>
    {
        public Guid Id { get; set; }
    }
}
