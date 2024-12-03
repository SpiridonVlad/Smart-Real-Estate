import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { ListingListComponent } from './listing-list.component';
import { ListingService } from '../../services/listing.service';
import { of } from 'rxjs';
import { Listing, ListingAsset } from '../../models/listing.model';

describe('ListingListComponent', () => {
  let component: ListingListComponent;
  let fixture: ComponentFixture<ListingListComponent>;
  let listingService: ListingService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, ListingListComponent],
      providers: [ListingService],
    }).compileComponents();

    fixture = TestBed.createComponent(ListingListComponent);
    component = fixture.componentInstance;
    listingService = TestBed.inject(ListingService);
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load listings on init', () => {
    const mockListings: Listing[] = [
      {
        id: '1',
        propertyId: '123',
        userId: '456',
        description: 'Test Listing',
        price: 100,
        publicationDate: new Date(),
        properties: [ListingAsset.IsHighlighted, ListingAsset.IsSold],
      },
    ];

    spyOn(listingService, 'getPaginatedListings').and.returnValue(of({ data: mockListings }));

    component.ngOnInit();

    expect(component.listings).toEqual(mockListings);
  });

  it('should navigate to create listing', () => {
    spyOn(router, 'navigate');
    component.navigateToCreate();
    expect(router.navigate).toHaveBeenCalledWith(['/listings/create']);
  });

  it('should navigate to update listing', () => {
    spyOn(router, 'navigate');
    component.navigateToUpdate('1');
    expect(router.navigate).toHaveBeenCalledWith(['/listings/update', '1']);
  });

  it('should delete listing', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    spyOn(listingService, 'deleteListing').and.returnValue(of(null));
    spyOn(component, 'loadListings');

    component.deleteListing('1');

    expect(listingService.deleteListing).toHaveBeenCalledWith('1');
    expect(component.loadListings).toHaveBeenCalled();
  });
});
