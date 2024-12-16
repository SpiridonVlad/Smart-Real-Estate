import { Address } from "./address.model";

export interface Property {
    id: string;
    addressId: string;
    address: Address;
    imageId: string;
    userId: string;
    type: string;
    features: { [key: string]: number };
  }
