import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'],
  imports: [CommonModule, FormsModule]
  
})
export class UserProfileComponent implements OnInit {
  userDetails: User | null = null;
  route: any;

  constructor(
    private router: Router,
    private userService: UserService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const userId = this.authService.getUserId();

    if (userId) {
      this.userService.getUserById(userId).subscribe(
        (response) => {
          this.userDetails = response.data;
        },
        (error) => {
          console.error('Error fetching user details:', error);
        }
      );
    } else {
      console.error('User ID not found in token');
    }
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
        this.loadUserDetails(); // Reload user details after update
      },
      error: (error) => console.error('Error updating user:', error),
    });
  }
  loadUserDetails(): void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (userId) {
      this.userService.getUserById(userId).subscribe({
        next: (response) => {
          this.userDetails = response.data;
        },
        error: (error) => console.error('Error fetching user details:', error),
      });
    }
  }
  navigateTo(basePath: string, userId: string | undefined): void {
    if (userId) {
      this.router.navigate([basePath, userId]);
    } else {
      console.error('User ID is undefined!');
    }
  }
}
