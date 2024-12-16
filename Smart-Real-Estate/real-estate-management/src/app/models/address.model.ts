export interface Address {
    id: string;
    street: string;
    city: string;
    state: string;
    postalCode?: string;
    country: string;
    additionalInfo?: string;
  }
