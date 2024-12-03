export enum ListingAsset {
    IsSold = 'IsSold',
    IsHighlighted = 'IsHighlighted',
    IsDeleted = 'IsDeleted'
}

export interface Listing {
    id: string;
    propertyId: string;
    userId: string;
    description?: string;
    price: number;
    publicationDate: Date;
    properties: ListingAsset[];
}