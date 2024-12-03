import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ListingService } from './listing.service';
import { Listing, ListingAsset } from '../models/listing.model';

describe('ListingService', () => {
  let service: ListingService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ListingService]
    });

    service = TestBed.inject(ListingService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch paginated listings', () => {
    const listings: Listing[] = [
      {
        id: '1',
        propertyId: '101',
        userId: '201',
        description: 'Test listing',
        price: 250000,
        publicationDate: new Date('2023-12-01'),
        properties: [ListingAsset.IsHighlighted]
      }
    ];

    service.getPaginatedListings(1, 5).subscribe(response => {
      expect(response.data).toEqual(listings);
    });

    const req = httpMock.expectOne(request => request.url === service['apiUrl'] && request.method === 'GET');
    expect(req.request.method).toBe('GET');
    req.flush({ data: listings});
  });

  it('should create a listing', () => {
    const newListing: Listing = {
      id: '2',
      propertyId: '102',
      userId: '202',
      description: 'New listing',
      price: 300000,
      publicationDate: new Date('2023-12-02'),
      properties: [ListingAsset.IsSold]
    };

    service.createListing(newListing).subscribe(response => {
      expect(response).toEqual(newListing);
    });

    const req = httpMock.expectOne(service['apiUrl']);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newListing);
    req.flush(newListing);
  });

  it('should get a listing by id', () => {
    const listing: Listing = {
      id: '1',
      propertyId: '101',
      userId: '201',
      description: 'Test listing',
      price: 250000,
      publicationDate: new Date('2023-12-01'),
      properties: [ListingAsset.IsHighlighted]
    };

    service.getListingById('1').subscribe(response => {
      expect(response).toEqual(listing);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(listing);
  });

  it('should update a listing', () => {
    const updatedListing: Listing = {
      id: '1',
      propertyId: '101',
      userId: '201',
      description: 'Updated listing',
      price: 275000,
      publicationDate: new Date('2023-12-01'),
      properties: [ListingAsset.IsHighlighted]
    };

    service.updateListing('1', updatedListing).subscribe(response => {
      expect(response).toEqual(updatedListing);
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedListing);
    req.flush(updatedListing);
  });

  it('should delete a listing', () => {
    service.deleteListing('1').subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });
});
