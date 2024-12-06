import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  page: number = 1;
  pageSize: number = 5;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getPaginatedUsers(this.page, this.pageSize).subscribe(
      (response: any) => {
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