import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router, ActivatedRoute } from '@angular/router';
import { ListingUpdateComponent } from './listing-update.component';
import { ListingService } from '../../services/listing.service';
import { of } from 'rxjs';
import { Listing, ListingAsset } from '../../models/listing.model';

describe('ListingUpdateComponent', () => {
  let component: ListingUpdateComponent;
  let fixture: ComponentFixture<ListingUpdateComponent>;
  let listingService: ListingService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule],
      declarations: [ListingUpdateComponent],
      providers: [
        ListingService,
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: () => '1', // Mock listing ID
              },
            },
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ListingUpdateComponent);
    component = fixture.componentInstance;
    listingService = TestBed.inject(ListingService);
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load listing on init', () => {
    const listing: Listing = {
      id: '1',
      propertyId: '123',
      userId: '456',
      description: 'Test Description',
      price: 200000,
      publicationDate: new Date('2024-12-01'),
      properties: [ListingAsset.IsHighlighted, ListingAsset.IsSold],
    };

    spyOn(listingService, 'getListingById').and.returnValue(of(listing));

    component.ngOnInit();

    expect(component.listingForm.value).toEqual({
      propertyId: listing.propertyId,
      userId: listing.userId,
      description: listing.description,
      price: listing.price,
      publicationDate: listing.publicationDate.toISOString().split('T')[0], // ISO Date string
      properties: listing.properties.join(', '), // Assume properties are stored as a comma-separated string in the form
    });
  });

  it('should update listing', () => {
    const listing: Listing = {
      id: '1',
      propertyId: '123',
      userId: '456',
      description: 'Updated Description',
      price: 250000,
      publicationDate: new Date('2024-12-02'),
      properties: [ListingAsset.IsDeleted, ListingAsset.IsSold],
    };

    spyOn(listingService, 'updateListing').and.returnValue(of(listing));
    component.listingForm.patchValue({
      propertyId: listing.propertyId,
      userId: listing.userId,
      description: listing.description,
      price: listing.price,
      publicationDate: listing.publicationDate.toISOString().split('T')[0],
      properties: listing.properties.join(', '),
    });

    component.onSubmit();

    expect(listingService.updateListing).toHaveBeenCalledWith('1', {
      id: '1',
      propertyId: '123',
      userId: '456',
      description: 'Updated Description',
      price: 250000,
      publicationDate: new Date('2024-12-02'),
      properties: [ListingAsset.IsDeleted, ListingAsset.IsSold],
    });
  });

  it('should navigate after update', () => {
    const listing: Listing = {
      id: '1',
      propertyId: '123',
      userId: '456',
      description: 'Updated Description',
      price: 250000,
      publicationDate: new Date('2024-12-02'),
      properties: [ListingAsset.IsDeleted, ListingAsset.IsSold],
    };

    spyOn(listingService, 'updateListing').and.returnValue(of(listing));
    spyOn(router, 'navigate');
    component.listingForm.patchValue({
      propertyId: listing.propertyId,
      userId: listing.userId,
      description: listing.description,
      price: listing.price,
      publicationDate: listing.publicationDate.toISOString().split('T')[0],
      properties: listing.properties.join(', '),
    });

    component.onSubmit();

    expect(router.navigate).toHaveBeenCalledWith(['/listings']);
  });
});
