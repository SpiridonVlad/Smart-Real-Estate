﻿using Domain.Common;
using Domain.Entities;
using Domain.Types;
using MediatR;

namespace Application.Use_Cases.Commands
{
    public class UpdatePropertyCommand :  IRequest<Result<string>>
    {
        public Guid Id { get; set; }
        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }
        public string? ImageId { get; set; }
        public Guid? UserId { get; set; }
        public PropertyType? Type { get; set; }
        public Dictionary<PropertyFeatureType, int>? Features { get; set; }
    }
}
