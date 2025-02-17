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
import { HeaderComponent } from "../header/header.component";
import { FooterComponent } from "../footer/footer.component";
import { RouterModule } from '@angular/router';
import { MessageService } from '../../services/message.service';
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    HeaderComponent,
    RouterModule,
    FooterComponent
  ]
})
export class UserProfileComponent implements OnInit {
  isCurrentUser: boolean = false;
  userDetails: User | null = null;
  currentSection: string = 'messages';
  properties: Property[] = [];
  chats: any[] = []; // Add chats arrayy
  listings: Listing[] = [];
  isProfileActionsVisible: boolean = false;
  loading: boolean = true; // Add loading property
  constructor(
    private router: Router,
    private route: ActivatedRoute, // ActivatedRoute to get route params
    private propertyService: PropertyService,
    private userService: UserService,
    private authService: AuthService,
    private listingService: ListingService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const userIdFromRoute = this.route.snapshot.paramMap.get('id'); // Get the 'id' from the route
    const jwtUserId = this.authService.getUserId();
    const userId = userIdFromRoute ?? jwtUserId;
    this.isCurrentUser = !userIdFromRoute || userIdFromRoute === jwtUserId;
    if (!userIdFromRoute || userIdFromRoute === jwtUserId) {
      this.isProfileActionsVisible = true; // Show profile actions if the ids match
    }
    if (userId) {
      this.loadProperties(userId); // Use the route's userId
      this.loadListingsByUserId(userId);
      this.loadChats(); // Load chats
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
      this.router.navigate([`/listings/update/${listing.id}`]);
    } else {
      console.error('Property ID is missing or invalid.');
    }
  }

  deleteListing(listingId: string): void {
    if (confirm('Are you sure you want to delete this listing?')) {
      this.listingService.deleteListing(listingId).subscribe(
        () => {
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

  getStars(rating: number): Array<number> {
    return Array(Math.round(rating)).fill(1);
  }

  deleteProperty(propertyId: string): void {
    if (confirm('Are you sure you want to delete this listing?')) {
      this.propertyService.deleteProperty(propertyId).subscribe(
        () => {
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
  loadChats(): void {
    this.messageService.getChats().subscribe({
      next: (response) => {
        const userId = this.authService.getUserId();
        this.chats = response.map((chat: any) => {
          const otherParticipantId = chat.participant1Id === userId ? chat.participant2Id : chat.participant1Id;
          this.userService.getUserById(otherParticipantId).subscribe({
            next: (userResponse) => {
              chat.otherParticipantUsername = userResponse.data.username;
            },
            error: (error) => console.error('Error fetching user details:', error),
          });
          return chat;
        });
      },
      error: (error) => console.error('Error fetching chats:', error),
    });
  }
  navigateToChat(chatId: string): void {
    this.router.navigate([`/messages/${chatId}`]);
  }
}
