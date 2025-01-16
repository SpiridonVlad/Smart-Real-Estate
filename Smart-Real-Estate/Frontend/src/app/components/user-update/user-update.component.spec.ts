import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router, ActivatedRoute } from '@angular/router';
import { UserUpdateComponent } from './user-update.component';
import { UserService } from '../../services/user.service';
import { of } from 'rxjs';
import { User, UserType } from '../../models/user.model';

describe('UserUpdateComponent', () => {
  let component: UserUpdateComponent;
  let fixture: ComponentFixture<UserUpdateComponent>;
  let userService: UserService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, UserUpdateComponent], // Import UserUpdateComponent here
      providers: [
        UserService,
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => '1' // Mock user ID
              }
            }
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(UserUpdateComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService);
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load user on init', () => {
    const user: User = {
      id: '1',
      username: 'user1',
      password: 'password1',
      email: 'user1@example.com',
      verified: true,
      rating: 5,
      type: UserType.Individual,
      propertyHistory: []
    };

    spyOn(userService, 'getUserById').and.returnValue(of(user));

    component.ngOnInit();

    expect(component.userForm.value).toEqual({
      username: user.username,
      password: user.password,
      email: user.email,
      verified: user.verified,
      rating: user.rating,
      type: 'Individual',
      propertyHistory: user.propertyHistory
    });
  });

  it('should update user', () => {
    const user: User = {
      id: '1',
      username: 'user1',
      password: 'password1',
      email: 'user1@example.com',
      verified: true,
      rating: 5,
      type: UserType.Individual,
      propertyHistory: []
    };

    spyOn(userService, 'updateUser').and.returnValue(of(user));
    component.userForm.patchValue(user);

    component.onSubmit();

    expect(userService.updateUser).toHaveBeenCalledWith('1', {
      id: '1',
      username: 'user1',
      password: 'password1',
      email: 'user1@example.com',
      verified: true,
      rating: 5,
      type: UserType.Individual, // Ensure the type is converted to number
      propertyHistory: []
    });
  });

  it('should navigate after update', () => {
    const user: User = {
      id: '1',
      username: 'user1',
      password: 'password1',
      email: 'user1@example.com',
      verified: true,
      rating: 5,
      type: UserType.Individual,
      propertyHistory: []
    };

    spyOn(userService, 'updateUser').and.returnValue(of(user));
    spyOn(router, 'navigate');
    component.userForm.patchValue(user);

    component.onSubmit();

    expect(router.navigate).toHaveBeenCalledWith(['/users']);
  });
});