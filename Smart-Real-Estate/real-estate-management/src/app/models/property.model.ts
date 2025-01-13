export interface Property {
  id?: string;
  addressId?: string;
  userId?: string;
  address: {
    street: string;
    city: string;
    state: string;
    postalCode: string;
    country: string;
    additionalInfo: string;
  };
  title: string;
  imageIds: string[];
  type: number;
  features: {
    Garden: number;
    Garage: number;
    Pool: number;
    Balcony: number;
    Rooms: number;
    Surface: number;
    Floor: number;
    Year: number;
    HeatingUnit: number;
    AirConditioning: number;
    Elevator: number;
    Furnished: number;
    Parking: number;
    Storage: number;
    Basement: number;
    Attic: number;
    Alarm: number;
    Intercom: number;
    VideoSurveillance: number;
    FireAlarm: number;
  };
}
