import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-property-create',
  templateUrl: './property-create.component.html',
  styleUrls: ['./property-create.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class PropertyCreateComponent implements OnInit {
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

  constructor(
    private fb: FormBuilder,
    private propertyService: PropertyService,
    private router: Router
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
      userId: ['', Validators.required],
      type: ['', Validators.required],
      features: this.fb.group(
        this.featuresList.reduce((acc: { [key: string]: number }, feature: string) => {
          acc[feature] = 0;
          return acc;
        }, {})
      )
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.propertyForm.valid) {
      const formData = this.propertyForm.value;
      formData.type = this.propertyTypes[formData.type]; // Convert type to its corresponding numeric value
      formData.features = { features: this.convertFeaturesToNumbers(formData.features) }; // Convert features to 0 or 1
      
      console.log('Form Data to send:', formData); // Log the form data for debugging
  
      this.propertyService.createProperty(formData).subscribe(
        response => {
          console.log('Property created:', response);
          this.router.navigate(['/properties']);
        },
        error => {
          console.error('Error creating property:', error);
          if (error.status === 400 && error.error && error.error.errors) {
            console.error('Validation errors:', error.error.errors);
          }
        }
      );
    }
  }

  private convertFeaturesToNumbers(features: { [key: string]: boolean }): { [key: string]: number } {
    return Object.keys(features).reduce((acc: { [key: string]: number }, key: string) => {
      acc[key] = features[key] ? 1 : 0;
      return acc;
    }, {});
  }
}