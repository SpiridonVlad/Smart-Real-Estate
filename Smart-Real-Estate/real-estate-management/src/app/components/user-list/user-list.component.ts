import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  page: number = 1;
  pageSize: number = 5;
  pages: number[] = [1, 2, 3, 4, 5]; // Example page numbers
  pageSizes: number[] = [5, 10, 20, 50]; // Example page sizes

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
}