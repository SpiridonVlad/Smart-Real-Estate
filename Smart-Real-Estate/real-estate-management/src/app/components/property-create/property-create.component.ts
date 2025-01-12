import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { CommonModule } from '@angular/common';
import { PricePredictionRequest } from '../../models/price-prediction.interface';
import { Property } from '../../models/property.model';
import { HttpClient,HttpClientModule } from '@angular/common/http';
@Component({
  selector: 'app-property-create',
  templateUrl: './property-create.component.html',
  styleUrls: ['./property-create.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule, HttpClientModule]
})
export class PropertyCreateComponent implements OnInit {
  propertyForm: FormGroup;
  featuresList: string[] = [
    'Garden', 'Garage', 'Pool', 'Balcony', 'Rooms', 'Surface', 'Floor', 'Year',
    'HeatingUnit', 'AirConditioning', 'Elevator', 'Furnished', 'Parking', 'Storage',
    'Basement', 'Attic', 'Alarm', 'Intercom', 'VideoSurveillance', 'FireAlarm'
  ];
  predictionResult: number | null = null;
  numericFeatures = ['Surface', 'Rooms', 'Floor', 'Year'];
  booleanFeatures = this.featuresList.filter(f => !this.numericFeatures.includes(f));
  propertyTypes: { [key: string]: number } = {
    'Apartment': 0,
    'Office': 1,
    'Studio': 2,
    'CommercialSpace': 3,
    'House': 4,
    'Garage': 5
  };
  constructor(
    private fb: FormBuilder,
    private propertyService: PropertyService,
    private router: Router,
    private http: HttpClient
  ) {
    this.propertyForm = this.fb.group({
      address: this.fb.group({
        street: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        postalCode: ['', Validators.required],
        country: ['', Validators.required],
        additionalInfo: ['']
      }),
      title: ['', Validators.required],
      imageIds: [[]],
      type: [0, Validators.required],
      features: this.fb.group({
        Surface: [0, [Validators.required, Validators.min(0)]],
        Rooms: [0, [Validators.required, Validators.min(0)]],
        Floor: [0, [Validators.required, Validators.min(0)]],
        Year: [0, [Validators.required, Validators.min(1900)]],
        Garden: [false],
        Garage: [false],
        Pool: [false],
        Balcony: [false],
        HeatingUnit: [false],
        AirConditioning: [false],
        Elevator: [false],
        Furnished: [false],
        Parking: [false],
        Storage: [false],
        Basement: [false],
        Attic: [false],
        Alarm: [false],
        Intercom: [false],
        VideoSurveillance: [false],
        FireAlarm: [false]
      }),
      price: [0, Validators.required],
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.propertyForm.valid) {
      const formData = this.propertyForm.value;
      const property: Property = {
        address: {
          street: formData.address.street,
          city: formData.address.city,
          state: formData.address.state,
          postalCode: formData.address.postalCode,
          country: formData.address.country,
          additionalInfo: formData.address.additionalInfo
        },
        title: formData.title,
        imageIds: Array.isArray(formData.imageIds) ? formData.imageIds : [formData.imageIds],
        type: formData.type,
        features: this.convertFeaturesToNumbers(formData.features)
      };

      this.propertyService.createProperty(property).subscribe({
        //next: () => this.router.navigate(['/properties']),
        error: (error) => console.error('Error creating property:', error)
      });
    }
  }

  private convertFeaturesToNumbers(features: { [key: string]: boolean | number }): {
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
} {
    return {
        Surface: typeof features['Surface'] === 'number' ? features['Surface'] : 0,
        Rooms: typeof features['Rooms'] === 'number' ? features['Rooms'] : 0,
        Floor: typeof features['Floor'] === 'number' ? features['Floor'] : 0,
        Year: typeof features['Year'] === 'number' ? features['Year'] : 0,
        Garden: features['Garden'] ? 1 : 0,
        Garage: features['Garage'] ? 1 : 0,
        Pool: features['Pool'] ? 1 : 0,
        Balcony: features['Balcony'] ? 1 : 0,
        HeatingUnit: features['HeatingUnit'] ? 1 : 0,
        AirConditioning: features['AirConditioning'] ? 1 : 0,
        Elevator: features['Elevator'] ? 1 : 0,
        Furnished: features['Furnished'] ? 1 : 0,
        Parking: features['Parking'] ? 1 : 0,
        Storage: features['Storage'] ? 1 : 0,
        Basement: features['Basement'] ? 1 : 0,
        Attic: features['Attic'] ? 1 : 0,
        Alarm: features['Alarm'] ? 1 : 0,
        Intercom: features['Intercom'] ? 1 : 0,
        VideoSurveillance: features['VideoSurveillance'] ? 1 : 0,
        FireAlarm: features['FireAlarm'] ? 1 : 0
    };
}

  getPricePrediction(): void {

      const formData = this.propertyForm.value;
      console.log('Form data:', formData);
      const predictionRequest: PricePredictionRequest = {
        surface: formData.features.Surface || 0,
        rooms: formData.features.Rooms || 0,
        description: formData.address.additionalInfo || '',
        price: 0,
        address: `${formData.address.street}, ${formData.address.city}`,
        year: formData.features.Year || 0,
        parking: formData.features.Parking === 1,
        floor: formData.features.Floor || 0
      };

      this.http.post<number>('https://localhost:7117/api/v1/PropertyPricePrediction/predict',
        predictionRequest).subscribe(
        result => {
          this.predictionResult = result;
        },
        error => {
          console.error('Error predicting price:', error);
        }
      );

  }
}
