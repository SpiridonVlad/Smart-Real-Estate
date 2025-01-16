import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PropertyService } from './property.service';
import { Property } from '../models/property.model';

describe('PropertyService', () => {
  let service: PropertyService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PropertyService],
    });

    service = TestBed.inject(PropertyService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch paginated properties', () => {
    const properties: Property[] = [
      {
        id: '1',
        addressId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
        address: {
          street: '123 Main St',
          city: 'Anytown',
          state: 'Anystate',
          country: 'Anycountry',
        },
        imageId: 'image1',
        userId: 'user1',
        type: 'House',
        features: { bedrooms: 3, bathrooms: 2 },
      },
    ];

    service.getPaginatedProperties(1, 5).subscribe((response) => {
      expect(response).toEqual(properties);
    });

    const req = httpMock.expectOne((request) => 
      request.url === service['apiUrl'] &&
      request.method === 'GET' &&
      request.params.get('page') === '1' &&
      request.params.get('pageSize') === '5'
    );

    expect(req.request.method).toBe('GET');
    req.flush({ data: properties });
  });

  it('should get property by ID', () => {
    const property: Property = {
      id: '1',
      addressId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
      address: {
        street: '123 Main St',
        city: 'Anytown',
        state: 'Anystate',
        country: 'Anycountry',
      },
      imageId: 'image1',
      userId: 'user1',
      type: 'House',
      features: { bedrooms: 3, bathrooms: 2 },
    };

    service.getPropertyById('1').subscribe((response) => {
      expect(response).toEqual(property);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(property);
  });

  it('should create a property', () => {
    const newProperty: Property = {
      id: '2',
      addressId: 'e38b0929-0c06-4038-9a2c-343a482773db',
      address: {
        street: '456 Elm St',
        city: 'Othertown',
        state: 'Otherstate',
        country: 'Othercountry',
      },
      imageId: 'image2',
      userId: 'user2',
      type: 'Apartment',
      features: { bedrooms: 2, bathrooms: 1 },
    };

    service.createProperty(newProperty).subscribe((response) => {
      expect(response).toEqual(newProperty);
    });

    const req = httpMock.expectOne(service['apiUrl']);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newProperty);
    req.flush(newProperty);
  });

  it('should update a property', () => {
    const updatedProperty: Property = {
      id: '1',
      addressId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
      address: {
        street: '123 Main St Updated',
        city: 'Anytown',
        state: 'Anystate',
        country: 'Anycountry',
      },
      imageId: 'image1',
      userId: 'user1',
      type: 'House',
      features: { bedrooms: 4, bathrooms: 3 },
    };

    service.updateProperty(updatedProperty).subscribe((response) => {
      expect(response).toEqual(updatedProperty);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/${updatedProperty.id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedProperty);
    req.flush(updatedProperty);
  });

  it('should get address by ID (hardcoded)', () => {
    service.getAddressById('3fa85f64-5717-4562-b3fc-2c963f66afa6').subscribe((address) => {
      expect(address).toEqual({
        street: '123 Main St',
        city: 'Anytown',
        state: 'Anystate',
        country: 'Anycountry',
      });
    });
  });

  it('should return error when address ID is not found', () => {
    service.getAddressById('invalid-id').subscribe(
      () => fail('Expected an error'),
      (error) => expect(error).toBe('Address not found')
    );
  });
});
