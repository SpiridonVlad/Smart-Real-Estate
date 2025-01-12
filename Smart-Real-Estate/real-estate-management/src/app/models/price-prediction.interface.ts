export interface PricePredictionRequest {
  surface: number;
  rooms: number;
  description: string;
  price: number;
  address: string;
  year: number;
  parking: boolean;
  floor: number;
}
