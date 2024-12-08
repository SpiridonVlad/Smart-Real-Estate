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
  listingAssets = Object.keys(ListingAsset).map(key => ListingAsset[key as keyof typeof ListingAsset]); // Enum pentru proprietăți

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
      publicationDate: [new Date().toISOString(), Validators.required],
      features: this.fb.group({
        features: this.fb.group({
          IsSold: [0, Validators.required],
          IsHighlighted: [0, Validators.required],
          IsDeleted: [0, Validators.required],
        })
      })
    });
  }

  onSubmit(): void {
    if (this.listingForm.valid) {
      const listingData = {
        ...this.listingForm.value,
        features: this.listingForm.value.features.features
      }

      console.log('Listing data:', listingData);

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
  

  toggleAsset(asset: ListingAsset): void {
    const currentValue = this.listingForm.get('features')?.value[asset];
    const newValue = currentValue === 0 ? 1 : 0;

    this.listingForm.patchValue({
      features: {
        ...this.listingForm.value.features,
        [asset]: newValue
      }
    });
  }
}  
