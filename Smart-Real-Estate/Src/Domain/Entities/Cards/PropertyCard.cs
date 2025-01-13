﻿using Domain.Types;

namespace Domain.Entities.Cards
{
    public class PropertyCard
    {
        public required Guid Id { get; set; }
        public required string ImageId { get; set; }
        public PropertyType Type { get; set; }
        public Dictionary<PropertyFeatureType, int> Features { get; set; } = [];
    }
}
