using Domain.Common;
using Domain.Types;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Use_Cases.Commands
{
    public class CreateListingCommand : IRequest<Result<Guid>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid PropertyId { get; set; }
        public decimal Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? Description { get; set; }
        public Dictionary<ListingType, int>? Features { get; set; }
    }
}
