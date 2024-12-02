import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService } from './user.service';
import { User, UserType } from '../models/user.model';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService]
    });

    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch paginated users', () => {
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

    service.getPaginatedUsers(1, 5).subscribe(response => {
      expect(response.data).toEqual(users);
    });

    const req = httpMock.expectOne(request => request.url === service['apiUrl'] && request.method === 'GET');
    expect(req.request.method).toBe('GET');
    req.flush({ data: users });
  });

  it('should create a user', () => {
    const newUser: User = {
      id: '12345',
      username: 'testuser',
      password: 'password',
      email: 'test@example.com',
      verified: true,
      rating: 5,
      type: UserType.Individual,
      propertyHistory: []
    };

    service.createUser(newUser).subscribe(response => {
      expect(response).toEqual(newUser);
    });

    const req = httpMock.expectOne(service['apiUrl']);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newUser);
    req.flush(newUser);
  });

  it('should get a user by id', () => {
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

    service.getUserById('1').subscribe(response => {
      expect(response).toEqual(user);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(user);
  });

  it('should update a user', () => {
    const updatedUser: User = {
      id: '1',
      username: 'updateduser',
      password: 'updatedpassword',
      email: 'updated@example.com',
      verified: true,
      rating: 5,
      type: UserType.Individual,
      propertyHistory: []
    };

    service.updateUser('1', updatedUser).subscribe(response => {
      expect(response).toEqual(updatedUser);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedUser);
    req.flush(updatedUser);
  });

  it('should delete a user', () => {
    service.deleteUser('1').subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });
});