import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ListingService } from '../../services/listing.service';
import { ListingAsset } from '../../models/listing.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-listing-update',
  templateUrl: './listing-update.component.html',
  styleUrls: ['./listing-update.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class ListingUpdateComponent implements OnInit {
  listingForm: FormGroup;
  listingAssets = Object.keys(ListingAsset); // Enum pentru proprietăți
  listingId!: string;

  constructor(
    private fb: FormBuilder,
    private listingService: ListingService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.listingForm = this.fb.group({
      id: [{ value: '', disabled: true }], // ID readonly
      propertyId: ['', Validators.required],
      userId: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      publicationDate: ['', Validators.required],
      properties: [[], Validators.required]
    });
  }

  ngOnInit(): void {
    this.listingId = this.route.snapshot.paramMap.get('id')!;
    this.loadListing();
  }

  loadListing(): void {
    this.listingService.getListingById(this.listingId).subscribe(
      (listing) => {
        this.listingForm.patchValue({
          id: listing.id, // Populăm ID-ul
          propertyId: listing.propertyId,
          userId: listing.userId,
          description: listing.description,
          price: listing.price,
          publicationDate: listing.publicationDate,
          properties: listing.properties
        });
      },
      (error) => {
        console.error('Error loading listing:', error);
      }
    );
  }

  toggleAsset(asset: string): void {
    const properties = this.listingForm.get('properties')?.value || [];
    if (properties.includes(asset)) {
      this.listingForm.patchValue({
        properties: properties.filter((a: string) => a !== asset)
      });
    } else {
      this.listingForm.patchValue({
        properties: [...properties, asset]
      });
    }
  }

  onSubmit(): void {
    if (this.listingForm.valid) {
      const updatedListing = {
        id: this.listingId,
        ...this.listingForm.value,
        properties: this.listingForm.value.properties
      };
      this.listingService.updateListing(this.listingId, updatedListing).subscribe(
        () => {
          this.router.navigate(['/listings']);
        },
        (error) => {
          console.error('Error updating listing:', error);
        }
      );
    } else {
      console.log('Form is invalid:', this.listingForm.errors);
      console.log('Form controls:', this.listingForm.controls);
    }
  }
}
