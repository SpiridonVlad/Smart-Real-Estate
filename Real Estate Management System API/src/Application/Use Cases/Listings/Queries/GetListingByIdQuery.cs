using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetListingByIdQuery : IRequest<Result<ListingDto>>
    {
        public Guid Id { get; set; }
    }
}
