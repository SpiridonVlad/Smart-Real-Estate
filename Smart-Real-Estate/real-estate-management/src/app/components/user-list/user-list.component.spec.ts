import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { UserListComponent } from './user-list.component';
import { UserService } from '../../services/user.service';
import { of } from 'rxjs';
import { User, UserType } from '../../models/user.model';

describe('UserListComponent', () => {
  let component: UserListComponent;
  let fixture: ComponentFixture<UserListComponent>;
  let userService: UserService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, UserListComponent], // Import UserListComponent here
      providers: [UserService]
    }).compileComponents();

    fixture = TestBed.createComponent(UserListComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService);
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load users on init', () => {
    const users: User[] = [
      {
        id: '1',
        username: 'user1',
        password: 'password1',
        email: 'user1@example.com',
        verified: true,
        rating: 5,
        type: UserType.Individual,
        propertyHistory: []
      }
    ];

    spyOn(userService, 'getPaginatedUsers').and.returnValue(of({ data: users }));

    component.ngOnInit();

    expect(component.users).toEqual(users);
  });

  it('should navigate to create user', () => {
    spyOn(router, 'navigate');

    component.navigateToCreate();

    expect(router.navigate).toHaveBeenCalledWith(['/users/create']);
  });

  it('should navigate to update user', () => {
    spyOn(router, 'navigate');

    component.navigateToUpdate('1');

    expect(router.navigate).toHaveBeenCalledWith(['/users/update', '1']);
  });

  it('should delete a user', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    spyOn(userService, 'deleteUser').and.returnValue(of({}));
    spyOn(component, 'loadUsers');

    component.deleteUser('1');

    expect(userService.deleteUser).toHaveBeenCalledWith('1');
    expect(component.loadUsers).toHaveBeenCalled();
  });
});