export interface Listing {
  propertyId: string;
  price: number;
  publicationDate: string;
  description: string;
  features: {
    IsSold: number;
    IsHighlighted: number;
    IsDeleted: number;
    ForSale: number;
    ForRent: number;
    ForLease: number;
  };
}
