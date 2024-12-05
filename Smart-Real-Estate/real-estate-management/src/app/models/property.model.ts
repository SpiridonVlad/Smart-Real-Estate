export interface Property {
    id: string;
    addressId: string;
    address: {
      street: string;
      city: string;
      state: string;
      country: string;
    };
    imageId: string;
    userId: string;
    type: string;
    features: { [key: string]: number };
  }
