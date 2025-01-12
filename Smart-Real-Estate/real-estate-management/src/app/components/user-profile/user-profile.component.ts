import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Property } from '../../models/property.model'; 
import { PropertyService } from '../../services/property.service'; 
import { Listing } from '../../models/listing.model';
import { ListingService } from '../../services/listing.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
  imports: [CommonModule, FormsModule]
})
export class UserProfileComponent implements OnInit {
  userDetails: User | null = null;
  currentSection: string = 'messages';
  properties: Property[] = [];
  listings: Listing[] = [];
  isProfileActionsVisible: boolean = false;
  constructor(
    private router: Router,
    private route: ActivatedRoute, // ActivatedRoute to get route params
    private propertyService: PropertyService,
    private userService: UserService,
    private authService: AuthService,
    private listingService: ListingService
  ) {}

  ngOnInit(): void {
    const userIdFromRoute = this.route.snapshot.paramMap.get('id'); // Get the 'id' from the route
    const jwtUserId = this.authService.getUserId();
    const userId = userIdFromRoute ?? jwtUserId;
    if (!userIdFromRoute || userIdFromRoute === jwtUserId) {
      this.isProfileActionsVisible = true; // Show profile actions if the ids match
    }
    if (userId) {
      this.loadProperties(userId); // Use the route's userId
      this.loadListingsByUserId(userId);
      this.userService.getUserById(userId).subscribe(
        (response) => {
          this.userDetails = response.data;
        },
        (error) => {
          console.error('Error fetching user details:', error);
        }
      );
    } else {
      console.error('User ID not found in route');
    }
  }

  loadUserDetails(userId: string): void {
    this.userService.getUserById(userId).subscribe({
      next: (response) => {
        this.userDetails = response.data;
      },
      error: (error) => console.error('Error fetching user details:', error),
    });
  }

  loadProperties(userId: string): void {
    this.propertyService.getPropertiesByUserId(userId).subscribe({
      next: (response) => {
        this.properties = response.data; // Store the properties in the array
      },
      error: (error) => {
        console.error('Error fetching properties:', error);
      },
    });
  }
  loadListingsByUserId(userId: string): void {
    this.listingService.getListingsByUserId(userId).subscribe(
      (response) => {
        this.listings = response.data;
        console.log('Fetched listings:', this.listings);
      },
      (error) => {
        console.error('Error fetching listings:', error);
      }
    );
  }

  onUpdateUser(): void {
    if (!this.userDetails) return;

    const updatedUser: User = {
      ...this.userDetails, // Keep current user data
      username: this.userDetails.username + '_updated', // Modify any field for demonstration
      verified: !this.userDetails.verified // Toggle 'verified' status
    };

    this.userService.updateUser(this.userDetails.id, updatedUser).subscribe({
      next: () => {
        alert('User updated successfully!');
        if (this.userDetails?.id) {
          this.loadUserDetails(this.userDetails.id); // Reload user details after update
        }
      },
      error: (error) => console.error('Error updating user:', error),
    });
  }

  navigateTo(basePath: string, userId?: string | undefined): void {
    if (userId) {
      this.router.navigate([basePath, userId]);
    } else {
      this.router.navigate([basePath]);
    }
  }

  changeSection(section: string): void {
    this.currentSection = section;
  }

  getUserType(type: number): string {
    switch (type) {
      case 0:
        return 'Individual';
      case 1:
        return 'LegalEntity';
      case 2:
        return 'Admin';
      default:
        return 'Unknown';
    }
  }

  getPropertyType(type: number): string {
    const propertyTypes = [
      'Apartment',
      'Office',
      'Studio',
      'Commercial Space',
      'House',
      'Garage',
    ];
    return propertyTypes[type] ?? 'Unknown';
  }

  isNumericFeature(featureKey: string): boolean {
    const numericFeatures = ['Rooms', 'Surface', 'Floor', 'Year'];
    return numericFeatures.includes(featureKey);
  }

  getFeatureList(features: { [key: string]: number }): { key: string; value: number }[] {
    return Object.entries(features).map(([key, value]) => ({ key, value }));
  }
  updateListing(listing: Listing): void {
    if (listing.propertyId) {
      this.router.navigate([`/listings/update/${listing.propertyId}`]);
    } else {
      console.error('Property ID is missing or invalid.');
    }
  }
  
  deleteListing(listingId: string): void {
    if (confirm('Are you sure you want to delete this listing?')) {
      this.listingService.deleteListing(listingId).subscribe(
        () => {
          console.log('Listing deleted successfully');
          this.listings = this.listings.filter((listing) => listing.id !== listingId);
        },
        (error) => {
          console.error('Error deleting listing:', error);
        }
      );
    }
  }

  updateProperty(property: Property): void {
    if (property.id) {
      this.router.navigate([`/properties/update/${property.id}`]);
    } else {
      console.error('Property ID is missing or invalid.');
    }
  }
  
  deleteProperty(propertyId: string): void {
    if (confirm('Are you sure you want to delete this listing?')) {
      this.propertyService.deleteProperty(propertyId).subscribe(
        () => {
          console.log('Property deleted successfully');
          this.properties = this.properties.filter((property) => property.id !== propertyId);
        },
        (error) => {
          console.error('Error deleting listing:', error);
        }
      );
    }
  }
  
  makeListing(property: any): void {
    if(property.id){
      this.router.navigate([`/listings/create/${property.id}`]);
    }else{
      console.error('Property ID is missing or invalid.');
    }
  }
}
