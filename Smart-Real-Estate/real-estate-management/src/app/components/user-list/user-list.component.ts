import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User, UserType } from '../../models/user.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from "../header/header.component";
import { FooterComponent } from "../footer/footer.component";
@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, HeaderComponent, FooterComponent]
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  page: number = 1;
  pageSize: number = 5;
  pages: number[] = [1, 2, 3, 4, 5];
  pageSizes: number[] = [5, 10, 20, 50];
  showFilterPopup: boolean = false;
  minRating: number = 0;
  maxRating: number = 5;
  verified: boolean | null = null;
  type: number | null = null;
  username: string = '';

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getPaginatedUsers(this.page, this.pageSize).subscribe(
      (response) => {
        this.users = response.data;
      },
      (error) => {
        console.error('Error loading users:', error);
      }
    );
  }

  navigateToCreate(): void {
    this.router.navigate(['/users/create']);
  }

  navigateToUpdate(userId: string): void {
    this.router.navigate(['/users/update', userId]);
  }

  navigateToProfile(userId: string): void {
    this.router.navigate(['/users/profile/', userId]);
  }

  deleteUser(userId: string): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(userId).subscribe(
        () => {
          this.loadUsers(); // Reload the user list after deletion
        },
        (error) => {
          console.error('Error deleting user:', error);
        }
      );
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
  nextPage(): void {
    if (this.page < this.pages.length) {
      this.page++;
      this.loadUsers();
    }
  }
  sendMessage(userId: string,): void {
    this.router.navigate(['/users/message', userId]);
  }
  previousPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadUsers();
    }
  }
  toggleFilterPopup(): void {
    this.showFilterPopup = !this.showFilterPopup;
  }

  applyFilters(): void {
    const verified = this.verified !== null ? this.verified : undefined;
    const type = this.type !== null ? this.type : undefined;
    this.userService.getPaginatedUsers(this.page, this.pageSize, this.minRating, this.maxRating, verified, type, this.username).subscribe(
      (response) => {
        this.users = response.data;
        this.toggleFilterPopup();
      },
      (error) => {
        console.error('Error loading users:', error);
      }
    );
  }

  getStars(rating: number): number[] {
    return Array(rating).fill(0);
  }
}
