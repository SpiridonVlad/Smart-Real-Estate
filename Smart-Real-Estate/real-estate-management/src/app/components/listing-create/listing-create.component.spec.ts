import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { ListingCreateComponent } from './listing-create.component';
import { ListingService } from '../../services/listing.service';
import { ListingAsset } from '../../models/listing.model';

describe('ListingCreateComponent', () => {
  let component: ListingCreateComponent;
  let fixture: ComponentFixture<ListingCreateComponent>;
  let listingServiceMock: any;
  let routerMock: any;

  beforeEach(() => {
    listingServiceMock = {
      createListing: jasmine.createSpy('createListing').and.returnValue(of({})),
    };

    routerMock = {
      navigate: jasmine.createSpy('navigate'),
    };

    TestBed.configureTestingModule({
      declarations: [ListingCreateComponent],
      imports: [ReactiveFormsModule, HttpClientTestingModule],
      providers: [
        { provide: ListingService, useValue: listingServiceMock },
        { provide: Router, useValue: routerMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ListingCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    expect(component.listingForm.value).toEqual({
      propertyId: '',
      userId: '',
      description: '',
      price: 0,
      publicationDate: new Date().toISOString().substring(0, 10),
      properties: [],
    });
  });

  it('should submit the form if valid', () => {
    component.listingForm.patchValue({
      propertyId: '123',
      userId: '456',
      description: 'Test Listing',
      price: 100,
      publicationDate: new Date().toISOString().substring(0, 10),
      properties: [ListingAsset.IsDeleted],
    });

    component.onSubmit();

    expect(listingServiceMock.createListing).toHaveBeenCalledWith({
      propertyId: '123',
      userId: '456',
      description: 'Test Listing',
      price: 100,
      publicationDate: new Date().toISOString().substring(0, 10),
      properties: [ListingAsset.IsDeleted],
    });

    expect(routerMock.navigate).toHaveBeenCalledWith(['/listings']);
  });

  it('should not submit the form if invalid', () => {
    component.listingForm.patchValue({
      propertyId: '',
      userId: '',
      description: '',
      price: -10, // Invalid price
      publicationDate: '',
      properties: [],
    });

    component.onSubmit();

    expect(listingServiceMock.createListing).not.toHaveBeenCalled();
    expect(routerMock.navigate).not.toHaveBeenCalled();
  });

  it('should handle errors when submitting the form', () => {
    spyOn(listingServiceMock, 'createListing').and.returnValue(throwError('Error'));
    component.onSubmit();
    expect(component.errorMessage).toBe('Error creating listing');
  });
  

  it('should toggle properties correctly', () => {
    component.toggleAsset('Feature1');

    expect(component.listingForm.get('properties')?.value).toContain(ListingAsset.IsDeleted);

    component.toggleAsset('Feature1');

    expect(component.listingForm.get('properties')?.value).not.toContain(ListingAsset.IsDeleted);
  });
});
