import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-user-list',
  imports: [],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent implements OnInit{
  users: User[]=[];
  constructor(private userService: UserService, private router : Router) { }

  ngOnInit(): void {
    this.userService.getPaginatedUsers(1, 5).subscribe(users => {
      this.users = users;
    });
  }
}
