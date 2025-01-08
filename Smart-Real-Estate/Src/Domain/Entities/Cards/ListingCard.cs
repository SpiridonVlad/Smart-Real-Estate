﻿using Domain.Types;

namespace Domain.Entities.Cards
{
    public class ListingCard
    {
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public Dictionary<ListingType, int> Features { get; set; } = [];
    }
}
