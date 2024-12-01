import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-user-list',
  imports: [CommonModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent implements OnInit{

  users: User[]=[];
  constructor(private userService: UserService, private router : Router) { }

  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers(): void {
    this.userService.getPaginatedUsers(1, 5).subscribe(
      (response: any) => {
        this.users = response.data;
      },
      (error) => {
        console.error('Error loading users:', error);
      }
    );
  }
  navigateToCreate(){
    this.router.navigate(['/users/create']);
  }
}
