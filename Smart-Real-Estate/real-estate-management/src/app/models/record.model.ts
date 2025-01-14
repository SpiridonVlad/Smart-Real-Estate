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
    id: string;
    imageId: string;
    type: number;
    features: { [key: string]: number };
  }
  
  export interface UserCard {
    id: string;
    username: string;
    verified: boolean;
    rating: number;
    type: number;
  }
  
  export interface ListingCard {
    id: string;
    description?: string;
    price: number;
    publicationDate: Date;
    features:{ [key: string]: number };
  }
  
  export interface Record {
    address: Address
    property: PropertyCard;
    user: UserCard;
    listing: ListingCard;
  }
  