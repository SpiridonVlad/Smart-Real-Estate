import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ListingService } from '../../services/listing.service';
import { ListingAsset } from '../../models/listing.model'; // Enum pentru asseturi
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { log } from 'console';

@Component({
  selector: 'app-listing-update',
  templateUrl: './listing-update.component.html',
  styleUrls: ['./listing-update.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class ListingUpdateComponent implements OnInit {
  listingForm: FormGroup;
  listingAssets = Object.keys(ListingAsset).map(key => ListingAsset[key as keyof typeof ListingAsset]);
  listingId!: string;

  constructor(
    private fb: FormBuilder,
    private listingService: ListingService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.listingForm = this.fb.group({
      propertyId: [{ value: '', disabled: true }, Validators.required],
      userId: [{ value: '', disabled: true }, Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      publicationDate: ['', Validators.required],
      features: this.fb.group({
        IsSold: [0, Validators.required],
        IsHighlighted: [0, Validators.required],
        IsDeleted: [0, Validators.required],
      })
    });
  }

  ngOnInit(): void {
    // Se obține ID-ul din URL
    this.listingId = this.route.snapshot.paramMap.get('id')!;
    this.loadListing();
  }

  loadListing(): void {
    // Se preiau datele listing-ului folosind ID-ul
    this.listingService.getListingById(this.listingId).subscribe(
      (response) => {
        const listing = response.data;
        this.listingForm.patchValue({
          propertyId: listing.propertyId,
          userId: listing.userId,
          description: listing.description || '',
          price: listing.price || 0,
          publicationDate: listing.publicationDate || '',
          features: {
            IsSold: listing.features.features.IsSold || 0,
            IsHighlighted: listing.features.features.IsHighlighted || 0,
            IsDeleted: listing.features.features.IsDeleted || 0
          }
        });
      },
      (error) => {
        console.error('Error loading listing:', error);
      }
    );
  }

  onSubmit(): void {
    if (this.listingForm.valid) {
      const updatedListing = {
        id: this.listingId,
        propertyId: this.listingForm.get('propertyId')?.value,
        userId: this.listingForm.get('userId')?.value,
        description: this.listingForm.get('description')?.value,
        price: this.listingForm.get('price')?.value,
        publicationDate: this.listingForm.get('publicationDate')?.value,
        features: {
          features: {
            IsSold: this.listingForm.get('features.IsSold')?.value,
            IsHighlighted: this.listingForm.get('features.IsHighlighted')?.value,
            IsDeleted: this.listingForm.get('features.IsDeleted')?.value
          }
        }
      }
      
      console.log('Updated listing:', updatedListing);

      this.listingService.updateListing(this.listingId, updatedListing).subscribe(
        () => {
          this.router.navigate(['/listings']);
        },
        (error) => {
          console.error('Error updating listing:', error);
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
