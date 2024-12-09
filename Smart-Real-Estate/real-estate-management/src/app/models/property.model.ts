export enum PropertyFeature {
    Garden = 'Garden',
    Garage = 'Garage',
    Pool = 'Pool',
    Balcony = 'Balcony',
    Rooms = 'Rooms',
    Surface = 'Surface',
    Floor = 'Floor',
    Year = 'Year',
    HeatingUnit = 'HeatingUnit',
    AirConditioning = 'AirConditioning',
    Elevator = 'Elevator',
    Furnished = 'Furnished',
    Parking = 'Parking',
    Storage = 'Storage',
    Basement = 'Basement',
    Attic = 'Attic',
    Alarm = 'Alarm',
    Intercom = 'Intercom',
    VideoSurveillance = 'VideoSurveillance',
    FireAlarm = 'FireAlarm'
}

export enum PropertyType {
  Apartment = 'Apartment',
  Office = 'Office',
  Studio = 'Studio',
  CommercialSpace = 'CommercialSpace',
  House = 'House',
  Garage = 'Garage'
}

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
    type: PropertyType;
    features: { 
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
      }
     };
  }