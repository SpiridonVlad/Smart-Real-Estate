import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { ListingUpdateComponent } from './listing-update.component';
import { ListingService } from '../../services/listing.service';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from "../header/header.component";
import { FooterComponent } from '../footer/footer.component';

describe('ListingUpdateComponent', () => {
  let component: ListingUpdateComponent;
  let fixture: ComponentFixture<ListingUpdateComponent>;
  let listingService: jasmine.SpyObj<ListingService>;
  let router: jasmine.SpyObj<Router>;
  let activatedRoute: ActivatedRoute;

  beforeEach(async () => {
    const listingServiceSpy = jasmine.createSpyObj('ListingService', ['getListingById', 'updateListing']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [ListingUpdateComponent],
      imports: [ReactiveFormsModule, FormsModule, CommonModule, HeaderComponent, FooterComponent],
      providers: [
        { provide: ListingService, useValue: listingServiceSpy },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: { paramMap: { get: () => '123' } }
          }
        }
      ]
    }).compileComponents();

    listingService = TestBed.inject(ListingService) as jasmine.SpyObj<ListingService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    activatedRoute = TestBed.inject(ActivatedRoute);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListingUpdateComponent);
    component = fixture.componentInstance;
    listingService.getListingById.and.returnValue(of({ data: { userId: '1', publicationDate: '2025-01-12T14:08:13.032Z', propertyId: '1', price: 2000, description: 'Test', features: { IsSold: 1, IsHighlighted: 1, IsDeleted: 1, ForSale: 1, ForRent: 1, ForLease: 1 } } }));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load listing on init', () => {
    expect(component.userId).toBe('1');
    expect(component.publicationDate).toBe('2025-01-12T14:08:13.032Z');
    expect(component.listingForm.value).toEqual({
      propertyId: '1',
      price: 2000,
      description: 'Test',
      features: {
        IsSold: true,
        IsHighlighted: true,
        IsDeleted: true,
        ForSale: true,
        ForRent: true,
        ForLease: true
      }
    });
  });

  it('should update listing on submit', () => {
    component.listingForm.setValue({
      propertyId: '1',
      price: 2500,
      description: 'Updated Test',
      features: {
        IsSold: false,
        IsHighlighted: false,
        IsDeleted: false,
        ForSale: false,
        ForRent: false,
        ForLease: false
      }
    });

    component.onSubmit();

    expect(listingService.updateListing).toHaveBeenCalledWith('123', {
      id: '123',
      propertyId: '1',
      userId: '1',
      price: 2500,
      publicationDate: '2025-01-12T14:08:13.032Z',
      description: 'Updated Test',
      features: {
        IsSold: 0,
        IsHighlighted: 0,
        IsDeleted: 0,
        ForSale: 0,
        ForRent: 0,
        ForLease: 0
      }
    });
    expect(router.navigate).toHaveBeenCalledWith(['/listings']);
  });
});
