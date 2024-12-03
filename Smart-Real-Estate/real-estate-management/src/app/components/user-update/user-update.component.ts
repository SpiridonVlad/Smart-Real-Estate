import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User, UserType } from '../../models/user.model';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-update',
  templateUrl: './user-update.component.html',
  styleUrls: ['./user-update.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class UserUpdateComponent implements OnInit {
  userForm: FormGroup;
  userTypes = Object.keys(UserType).filter(key => isNaN(Number(key))); // Get string keys of UserType enum
  userId!: string;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.userForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      verified: [false, Validators.required],
      rating: [0, [Validators.required, Validators.min(0)]],
      type: [UserType.Individual, Validators.required],
      propertyHistory: [[]]
    });
  }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('id')!;
    this.loadUser();
  }

  loadUser(): void {
    this.userService.getUserById(this.userId).subscribe(
      (user: User) => {
        this.userForm.patchValue({
          username: user.username,
          password: user.password,
          email: user.email,
          verified: user.verified,
          rating: user.rating,
          type: UserType[user.type],
          propertyHistory: user.propertyHistory
        });
      },
      (error) => {
        console.error('Error loading user:', error);
      }
    );
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      const userData = {
        id: this.userId, // Add the id directly to userData
        ...this.userForm.value,
        type: this.mapUserType(this.userForm.value.type)
      };
      this.userService.updateUser(this.userId, userData).subscribe(
        () => {
          this.router.navigate(['/users']);
        },
        (error) => {
          console.error('Error updating user:', error);
        }
      );
    } else {
      console.log('Form is invalid:', this.userForm.errors);
      console.log('Form controls:', this.userForm.controls);
    }
  }

  private mapUserType(type: string): number {
    switch (type) {
      case 'Individual':
        return 0;
      case 'LegalEntity':
        return 1;
      case 'Admin':
        return 2;
      default:
        return 0; // Default to Individual if type is not recognized
    }
  }
}