

using Domain.Common;
using Domain.Entities;
using Domain.Types;
using MediatR;

namespace Application.Use_Cases.Commands
{
    public class CreatePropertyCommand : IRequest<Result<Guid>>
    {
        public Guid AddressId { get; set; }
        public required Address Address { get; set; }
        public required string ImageId { get; set; }
        public Guid UserId { get; set; }
        public PropertyType Type { get; set; }
        public required PropertyFeatures Features { get; set; }
    }
}
