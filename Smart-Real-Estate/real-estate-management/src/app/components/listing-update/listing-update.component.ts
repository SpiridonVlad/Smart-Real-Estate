import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ListingService } from '../../services/listing.service';
import { Listing } from '../../models/listing.model';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@Component({
  selector: 'app-listing-update',
  templateUrl: './listing-update.component.html',
  styleUrls: ['./listing-update.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class ListingUpdateComponent implements OnInit {
  listingForm: FormGroup;
  listingId!: string;
  featuresList = ['IsSold', 'IsHighlighted', 'IsDeleted', 'ForSale', 'ForRent', 'ForLease'];

  constructor(
    private fb: FormBuilder,
    private listingService: ListingService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.listingForm = this.fb.group({
      propertyId: [''],
      price: [0],
      publicationDate: [''],
      description: [''],
      features: this.fb.group({
        IsSold: [false],
        IsHighlighted: [false],
        IsDeleted: [false],
        ForSale: [false],
        ForRent: [false],
        ForLease: [false]
      })
    });
  }

  ngOnInit(): void {
    this.listingId = this.route.snapshot.paramMap.get('id')!;
    this.loadListing();
  }

  loadListing(): void {
    this.listingService.getListingById(this.listingId).subscribe(
      (response) => {
        const listing = response.data;
        this.listingForm.patchValue({
          propertyId: listing.propertyId,
          price: listing.price,
          publicationDate: listing.publicationDate,
          description: listing.description,
          features: {
            IsSold: listing.features.IsSold === 1,
            IsHighlighted: listing.features.IsHighlighted === 1,
            IsDeleted: listing.features.IsDeleted === 1,
            ForSale: listing.features.ForSale === 1,
            ForRent: listing.features.ForRent === 1,
            ForLease: listing.features.ForLease === 1
          }
        });
      }
    );
  }

  onSubmit(): void {
    if (this.listingForm.valid) {
      const formData = this.listingForm.value;
      const listing: Listing = {
        ...formData,
        features: this.convertFeaturesToNumbers(formData.features)
      };

      this.listingService.updateListing(this.listingId, listing).subscribe({
        next: () => this.router.navigate(['/listings']),
        error: (error) => console.error('Error updating listing:', error)
      });
    }
  }

  private convertFeaturesToNumbers(features: { [key: string]: boolean }): { [key: string]: number } {
    return {
      IsSold: features['IsSold'] ? 1 : 0,
      IsHighlighted: features['IsHighlighted'] ? 1 : 0,
      IsDeleted: features['IsDeleted'] ? 1 : 0,
      ForSale: features['ForSale'] ? 1 : 0,
      ForRent: features['ForRent'] ? 1 : 0,
      ForLease: features['ForLease'] ? 1 : 0
    };
  }
}
