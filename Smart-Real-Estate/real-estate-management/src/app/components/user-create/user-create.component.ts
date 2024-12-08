import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { UserType } from '../../models/user.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-create',
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule]
})
export class UserCreateComponent {
  userForm: FormGroup;
  userTypes = Object.keys(UserType).filter(key => isNaN(Number(key)));

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      type: [UserType.Individual, Validators.required],
      propertyHistory: [null],
    });
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      const userData = {
        ...this.userForm.value,
        type: this.mapUserType(this.userForm.value.type)
      };

      console.log('User data:', userData);
      
      this.userService.createUser(userData).subscribe(
        () => {
          this.router.navigate(['/users']);
        },
        (error) => {
          console.error('Error creating user:', error);
        }
      );
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