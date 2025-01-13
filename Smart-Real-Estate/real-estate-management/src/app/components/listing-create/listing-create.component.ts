import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ListingService } from '../../services/listing.service';
import { CommonModule } from '@angular/common';
import { Listing } from '../../models/listing.model';
import { OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { Property } from '../../models/property.model';
import { HttpClient } from '@angular/common/http';
import { PricePredictionRequest } from '../../models/price-prediction.interface';

@Component({
  selector: 'app-listing-create',
  templateUrl: './listing-create.component.html',
  styleUrls: ['./listing-create.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class ListingCreateComponent implements OnInit {
  propertyId:string = '';
  listingForm: FormGroup;
  featuresList = [ 'ForSale', 'ForRent', 'ForLease'];
  predictionResult: number | null = null;

  constructor(
    private fb: FormBuilder,
    private listingService: ListingService,
    private router: Router,
    private route: ActivatedRoute,
    private propertyService: PropertyService,
    private http: HttpClient
  ) {
    this.listingForm = this.fb.group({
      price: [0],
      description: [''],
      features: this.fb.group({
        IsSold: [0],
        IsHighlighted: [0],
        IsDeleted: [0],
        ForSale: [0],
        ForRent: [0],
        ForLease: [0]
      })
    });
  }
  ngOnInit(): void {

    const propertyId = this.route.snapshot.paramMap.get('id');
    console.log('Property ID:', propertyId); // Debug log
    if (!propertyId) {
      console.error('No property ID provided');
      return;
    }

    this.propertyId = propertyId;
    this.propertyService.getPropertyById(this.propertyId).subscribe({
      next: (response) => {
        const property = response.data;
        this.getPricePrediction(property);
      },
      error: (error) => console.error('Error fetching property:', error)
    });
  }
  onSubmit(): void {
    if (this.listingForm.valid) {
      const formData = this.listingForm.value;
      const listing: Listing = {
        propertyId: this.propertyId,
        price: formData.price,
        publicationDate: new Date().toISOString(),
        description: formData.description,
        features: {
          IsSold: 0,
          IsHighlighted: 0,
          IsDeleted:  0,
          ForSale: formData.features.ForSale ? 1 : 0,
          ForRent: formData.features.ForRent ? 1 : 0,
          ForLease: formData.features.ForLease ? 1 : 0
        }
      };
      console.log('Listing to submit:', listing); // Debug log

      this.listingService.createListing(listing).subscribe({
        next: () => this.router.navigate(['/records']),
        error: (error) => console.error('Error creating listing:', error)
      });
    }
  }

  private convertFeaturesToNumbers(features: { [key: string]: boolean }): {
    IsSold: number;
    IsHighlighted: number;
    IsDeleted: number;
    ForSale: number;
    ForRent: number;
    ForLease: number;
}{
  return {
      IsSold: features['IsSold'] ? 1 : 0,
      IsHighlighted: features['IsHighlighted'] ? 1 : 0,
      IsDeleted: features['IsDeleted'] ? 1 : 0,
      ForSale: features['ForSale'] ? 1 : 0,
      ForRent: features['ForRent'] ? 1 : 0,
      ForLease: features['ForLease'] ? 1 : 0
  };
}
private getPricePrediction(property: Property): void {
  const predictionRequest: PricePredictionRequest = {
    surface: property.features.Surface || 0,
    rooms: property.features.Rooms || 0,
    description: property.address.additionalInfo || '',
    price: 0,
    address: `${property.address.street}, ${property.address.city}`,
    year: property.features.Year || 0,
    parking: property.features.Parking === 1,
    floor: property.features.Floor || 0
  };

  this.http.post<number>('https://localhost:7117/api/v1/PropertyPricePrediction/predict',
    predictionRequest).subscribe({
      next: (result) => {
        this.predictionResult = result;
        this.updatePrice(Math.round(result));
      },
      error: (error) => console.error('Error predicting price:', error)
    });
}
private updatePrice(price: number): void {
  this.listingForm.patchValue({
    price: price
  });
}
}


