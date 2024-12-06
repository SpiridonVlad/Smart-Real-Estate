import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ListingService } from '../../services/listing.service';
import { ListingAsset } from '../../models/listing.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-listing-create',
  templateUrl: './listing-create.component.html',
  styleUrls: ['./listing-create.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class ListingCreateComponent {
  listingForm: FormGroup;
  listingAssets = Object.keys(ListingAsset);

  constructor(
    private fb: FormBuilder,
    private listingService: ListingService,
    private router: Router
  ) {
    this.listingForm = this.fb.group({
      propertyId: ['', Validators.required],
      userId: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      publicationDate: [new Date().toISOString().substring(0, 10), Validators.required],
      properties: [[], Validators.required]
    });
  }

  onSubmit(): void {
    if (this.listingForm.valid) {
      const listingData = {
        ...this.listingForm.value,
        property: this.listingForm.value.property,
      };

      this.listingService.createListing(listingData).subscribe(
        () => {
          this.router.navigate(['/listings']);
        },
        (error) => {
          console.error('Error creating listing:', error);
        }
      );
    }
  }
  

  toggleAsset(asset: string): void {
    const assetEnum = asset as ListingAsset; // Convertim stringul în tipul ListingAsset
    const properties = this.listingForm.get('properties')?.value || [];
    if (properties.includes(assetEnum)) {
      this.listingForm.patchValue({
        properties: properties.filter((a: ListingAsset) => a !== assetEnum)
      });
    } else {
      this.listingForm.patchValue({
        properties: [...properties, assetEnum]
      });
    }
  }

  navigateToListings(): void {
    this.router.navigate(['/listings']);
  }
}  
