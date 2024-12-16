import { Address } from "./address.model";

export enum PropertyType {
    Apartment = 'Apartment',
    Office = 'Office',
    Studio = 'Studio',
    CommercialSpace = 'CommercialSpace',
    House = 'House',
    Garage = 'Garage',
  }
  
  export enum UserType {
    LegalEntity = 'LegalEntity',
    Individual = 'Individual',
    Admin = 'Admin'
}
  
  export interface PropertyCard {
    imageId: string;
    pType: PropertyType;
    pFeatures: { [key: string]: number };
  }
  
  export interface UserCard {
    username: string;
    verified: boolean;
    rating: number;
    uType: UserType;
  }
  
  export interface ListingCard {
    description?: string;
    price: number;
    publicationDate: Date;
    lFeatures: {
        features: {
            IsSold: number;
            IsHighlighted: number;
            IsDeleted: number;
        }
    };
  }
  
  export interface Record {
    address: Address
    property: PropertyCard;
    user: UserCard;
    listing: ListingCard;
  }
  