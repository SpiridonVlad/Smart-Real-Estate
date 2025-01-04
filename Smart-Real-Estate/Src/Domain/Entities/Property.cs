﻿using Domain.Types;

namespace Domain.Entities
{
    public class Property
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public Guid AddressId { get; set; }
        public required Address Address { get; set; }
        public required List<string> ImageIds { get; set; } 
        public Guid UserId { get; set; }
        public PropertyType Type { get; set; }
        public Dictionary<PropertyFeatureType, int> Features { get; set; } = [];
    }
}
