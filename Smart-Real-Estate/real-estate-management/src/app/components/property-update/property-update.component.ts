import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { CommonModule } from '@angular/common';
import { Property } from '../../models/property.model';
@Component({
  selector: 'app-property-update',
  templateUrl: './property-update.component.html',
  styleUrls: ['./property-update.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class PropertyUpdateComponent implements OnInit {
  propertyForm: FormGroup;
  propertyTypes: { [key: string]: number } = {
    'Apartment': 0,
    'Office': 1,
    'Studio': 2,
    'CommercialSpace': 3,
    'House': 4,
    'Garage': 5
  };
  featuresList: string[] = [
    'Garden', 'Garage', 'Pool', 'Balcony', 'Rooms', 'Surface', 'Floor', 'Year',
    'HeatingUnit', 'AirConditioning', 'Elevator', 'Furnished', 'Parking', 'Storage',
    'Basement', 'Attic', 'Alarm', 'Intercom', 'VideoSurveillance', 'FireAlarm'
  ];
  numericFeatures = ['Surface', 'Rooms', 'Floor', 'Year'];
  booleanFeatures = this.featuresList.filter(f => !this.numericFeatures.includes(f));
  propertyId: string = '';
  addressId: string = '';
  userId: string = '';
  constructor(
    private fb: FormBuilder,
    private propertyService: PropertyService,
    private router: Router,
    private route: ActivatedRoute
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
      imageIds: [[], Validators.required],
      type: [0, Validators.required],
      features: this.fb.group(
        this.featuresList.reduce((acc: { [key: string]: any }, feature: string) => {
          acc[feature] = this.numericFeatures.includes(feature) ? [0, Validators.required] : [false]; // Initialize numeric and boolean features
          return acc;
        }, {})
      )
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.propertyId = id;
      this.propertyService.getPropertyById(id).subscribe({
        next: (response) => {
          const property = response.data;
          this.addressId = property.addressId || '';
          this.userId = property.userId || '';
          this.propertyForm.patchValue({
            address: property.address,
            imageIds: property.imageIds,
            type: property.type,
            title: property.title,
            features: this.convertFeaturesToNumbers(property.features)
          });//s
        },
        error: (error) => console.error('Error fetching property:', error)
      });
    }
  }

  onSubmit(): void {
    if (this.propertyForm.valid) {
      const formData = this.propertyForm.value;
      const property: Property = {
        id: this.propertyId,
        addressId: this.addressId,
        userId: this.userId,
        title: formData.title,
        address: formData.address,
        imageIds: formData.imageIds,
        type: formData.type,
        features: this.convertFeaturesToNumbers(formData.features)
      };

      this.propertyService.updateProperty(property).subscribe({
        next: () => this.router.navigate(['/users/profile']),
        error: (error) => console.error('Error updating property:', error)
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
}
