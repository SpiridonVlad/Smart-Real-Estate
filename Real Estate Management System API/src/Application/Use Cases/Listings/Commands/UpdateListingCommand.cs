
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands
{
    public class UpdateListingCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? PublicationDate { get; set; }
        public bool? IsSold { get; set; }
        public bool? IsHighlighted { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
