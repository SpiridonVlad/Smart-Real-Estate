import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService } from '../../services/user.service';
import { User, UserType } from '../../models/user.model';

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

    service.createUser(newUser).subscribe((response: any) => {
      expect(response).toEqual(newUser);
    });

    const req = httpMock.expectOne((request) => request.url === service['apiUrl'] && request.method === 'POST');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newUser);
    req.flush(newUser);
  });
});