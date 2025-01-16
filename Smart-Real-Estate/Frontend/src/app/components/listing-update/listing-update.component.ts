import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ListingService } from '../../services/listing.service';
import { Listing } from '../../models/listing.model';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HeaderComponent } from "../header/header.component";
import { FooterComponent } from '../footer/footer.component';
@Component({
  selector: 'app-listing-update',
  templateUrl: './listing-update.component.html',
  styleUrls: ['./listing-update.component.css'],
  imports: [ReactiveFormsModule, CommonModule, FormsModule, HeaderComponent, FooterComponent]
})
export class ListingUpdateComponent implements OnInit {
  listingForm: FormGroup;
  listingId: string = '';
  userId: string = '';
  publicationDate: string = '';
  propertyId: string = '';
  featuresList: string[] = ['IsSold', 'IsHighlighted', 'ForSale', 'ForRent', 'ForLease']; // Define featuresList

  constructor(
    private fb: FormBuilder,
    private listingService: ListingService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.listingForm = this.fb.group({
      price: [1, Validators.required],
      description: ['', Validators.required],
      features: this.fb.group({
        IsSold: [false],
        IsHighlighted: [false],
        ForSale: [false],
        ForRent: [false],
        ForLease: [false]
      })
    });
  }

  ngOnInit(): void {
    this.listingId = this.route.snapshot.paramMap.get('id') || '';
    this.loadListing();
  }

  loadListing(): void {
    this.listingService.getListingById(this.listingId).subscribe(
      (response) => {
        const listing = response.data;
        this.userId = listing.userId || '';
        this.publicationDate = listing.publicationDate;
        this.propertyId = listing.propertyId || '';
        this.listingForm.patchValue({
          propertyId: listing.propertyId,
          price: listing.price,
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
      },
      (error) => console.error('Error fetching listing:', error)
    );
  }

  onSubmit(): void {
    if (this.listingForm.valid) {
      const formData = this.listingForm.value;
      const listing: Listing = {
        id: this.listingId,
        propertyId: this.propertyId,
        userId: this.userId,
        price: formData.price,
        publicationDate: this.publicationDate,
        description: formData.description,
        features: {
          ...this.convertFeaturesToNumbers(formData.features),
          IsDeleted: 0
        }
      };

      this.listingService.updateListing(this.listingId, listing).subscribe({
        next: () => this.router.navigate(['/users/profile']),
        error: (error) => console.error('Error updating listing:', error)
      });
    }
  }

  private convertFeaturesToNumbers(features: { [key: string]: boolean }): { IsSold: number; IsHighlighted: number; IsDeleted: number; ForSale: number; ForRent: number; ForLease: number } {
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
