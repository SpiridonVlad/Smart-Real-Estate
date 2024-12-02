import { Component, OnInit } from '@angular/core';
import { ListingService } from '../../services/listing.service';
import { Router } from '@angular/router';
import { Listing } from '../../models/listing.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-listing-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './listing-list.component.html',
  styleUrls: ['./listing-list.component.css'],
})
export class ListingListComponent implements OnInit {
  listings: Listing[] = [];
  page: number = 1;
  pageSize: number = 5;

  constructor(private listingService: ListingService, private router: Router) {}

  ngOnInit(): void {
    this.loadListings();
  }

  loadListings(): void {
    this.listingService.getPaginatedListings(this.page, this.pageSize).subscribe(
      (response: any) => {
        this.listings = response.data;
      },
      (error) => {
        console.error('Error loading listings:', error);
      }
    );
  }

  navigateToCreate(): void {
    this.router.navigate(['/listings/create']);
  }

  navigateToUpdate(listingId: string): void {
    this.router.navigate(['/listings/update', listingId]);
  }

  deleteListing(listingId: string): void {
    if (confirm('Are you sure you want to delete this listing?')) {
      this.listingService.deleteListing(listingId).subscribe(
        () => {
          this.loadListings(); // Reload the listing list after deletion
        },
        (error) => {
          console.error('Error deleting listing:', error);
        }
      );
    }
  }
}
