import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { PropertyListComponent } from './property-list.component';
import { PropertyService } from '../../services/property.service';
import { Property } from '../../models/property.model';

describe('PropertyListComponent', () => {
  let component: PropertyListComponent;
  let fixture: ComponentFixture<PropertyListComponent>;
  let propertyService: PropertyService;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, PropertyListComponent],
      providers: [PropertyService],
    }).compileComponents();

    fixture = TestBed.createComponent(PropertyListComponent);
    component = fixture.componentInstance;
    propertyService = TestBed.inject(PropertyService);
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load properties on init', () => {
    const mockProperties: Property[] = [
      {
        id: '1',
        addressId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
        address: { street: '', city: '', state: '', country: '' },
        imageId: 'img1',
        userId: 'user1',
        type: 'House',
        features: { bedrooms: 3, bathrooms: 2 },
      },
    ];

    spyOn(propertyService, 'getPaginatedProperties').and.returnValue(of(mockProperties));
    spyOn(propertyService, 'getAddressById').and.returnValue(of({ street: '123 Main St', city: 'Anytown', state: 'Anystate', country: 'Anycountry' }));

    component.ngOnInit();

    expect(component.properties).toEqual(mockProperties);
    expect(propertyService.getAddressById).toHaveBeenCalledWith('3fa85f64-5717-4562-b3fc-2c963f66afa6');
  });

  it('should handle error when loading properties', () => {
    spyOn(propertyService, 'getPaginatedProperties').and.returnValue(throwError('Error loading properties'));

    component.loadProperties();

    expect(component.properties).toEqual([]);
    // Optionally: Check if console.error was called
  });

  it('should navigate to create property', () => {
    spyOn(router, 'navigate');
    component.navigateToCreate();
    expect(router.navigate).toHaveBeenCalledWith(['/properties/create']);
  });

  it('should navigate to update property', () => {
    spyOn(router, 'navigate');
    component.navigateToUpdate('1');
    expect(router.navigate).toHaveBeenCalledWith(['/properties/update', '1']);
  });

  it('should map features correctly', () => {
    const features = { bedrooms: 3, bathrooms: 2 };
    const mappedFeatures = component.getFeatures(features);
    expect(mappedFeatures).toEqual([
      { key: 'bedrooms', value: 3 },
      { key: 'bathrooms', value: 2 },
    ]);
  });
});
