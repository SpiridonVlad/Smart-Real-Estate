﻿
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Use_Cases.Commands
{
    public class UpdateListingCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public Guid UserId { get; set; }
        public int? Price { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? Description { get; set; }
        public ListingFeatures? Features { get; set; }

    }
}
