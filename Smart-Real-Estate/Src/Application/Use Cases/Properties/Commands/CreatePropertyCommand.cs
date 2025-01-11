using Domain.Common;
using Domain.Entities;
using Domain.Types;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Use_Cases.Commands
{
    public class CreatePropertyCommand : IRequest<Result<Guid>>
    {
        public required Address Address { get; set; }
        public required string Title { get; set; }
        public required List<string> ImageIds { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        public PropertyType Type { get; set; }
        public required Dictionary<PropertyFeatureType, int> Features { get; set; }
    }
}
