import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { CommonModule } from '@angular/common';

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

  constructor(
    private fb: FormBuilder,
    private propertyService: PropertyService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.propertyForm = this.fb.group({
      addressId: ['', Validators.required],
      address: this.fb.group({
        street: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        postalCode: ['', Validators.required],
        country: ['', Validators.required],
        additionalInfo: ['']
      }),
      imageId: ['', Validators.required],
      type: ['', Validators.required],
      features: this.fb.group(
        this.featuresList.reduce((acc: { [key: string]: boolean }, feature: string) => {
          acc[feature] = false;
          return acc;
        }, {})
      )
    });
  }

  ngOnInit(): void {
    const propertyId = this.route.snapshot.paramMap.get('id');
    if (propertyId) {
      this.propertyService.getPropertyById(propertyId).subscribe(property => {
        this.propertyForm.patchValue(property);
        this.propertyForm.patchValue({
          features: this.convertFeaturesToBoolean(property.data.features)
        });
      });
    }
  }

  onSubmit(): void {
    if (this.propertyForm.valid) {
      const formData = this.propertyForm.value;
      formData.type = this.propertyTypes[formData.type]; // Convert type to its corresponding numeric value
      formData.features = { features: this.convertFeaturesToNumbers(formData.features) }; // Convert features to 0 or 1

      console.log('Form Data to send:', formData); // Log the form data for debugging

      this.propertyService.updateProperty(formData).subscribe(
        response => {
          console.log('Property updated:', response);
          this.router.navigate(['/properties']);
        },
        error => {
          console.error('Error updating property:', error);
          if (error.status === 400 && error.error && error.error.errors) {
            console.error('Validation errors:', error.error.errors);
          }
        }
      );
    }
  }

  private convertFeaturesToBoolean(features: { [key: string]: number }): { [key: string]: boolean } {
    return Object.keys(features).reduce((acc: { [key: string]: boolean }, key: string) => {
      acc[key] = features[key] === 1;
      return acc;
    }, {});
  }

  private convertFeaturesToNumbers(features: { [key: string]: boolean }): { [key: string]: number } {
    return Object.keys(features).reduce((acc: { [key: string]: number }, key: string) => {
      acc[key] = features[key] ? 1 : 0;
      return acc;
    }, {});
  }
}
