import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

import { PropertyCreateComponent } from './property-create.component';
import { PropertyService } from '../../services/property.service';

describe('PropertyCreateComponent', () => {
  let component: PropertyCreateComponent;
  let fixture: any;
  let propertyServiceMock: any;
  let routerMock: any;

  beforeEach(() => {
    propertyServiceMock = {
      createProperty: jasmine.createSpy('createProperty').and.returnValue(of({})),
    };

    routerMock = {
      navigate: jasmine.createSpy('navigate'),
    };

    TestBed.configureTestingModule({
      declarations: [],
      imports: [ReactiveFormsModule, HttpClientTestingModule],
      providers: [
        { provide: PropertyService, useValue: propertyServiceMock },
        { provide: Router, useValue: routerMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PropertyCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with default values', () => {
    expect(component.propertyForm.value).toEqual({
      addressId: '',
      address: {
        street: '',
        city: '',
        state: '',
        postalCode: '',
        country: '',
        additionalInfo: '',
      },
      imageId: '',
      userId: '',
      type: '',
      features: jasmine.any(Object),
    });
  });

  it('should submit the form if valid', () => {
    component.propertyForm.patchValue({
      addressId: '1',
      address: {
        street: 'Test Street',
        city: 'Test City',
        state: 'Test State',
        postalCode: '12345',
        country: 'Test Country',
        additionalInfo: '',
      },
      imageId: '10',
      userId: '5',
      type: 'Apartment',
      features: {
        Garden: true, // true => 1
        Pool: false,  // false => 0
      },
    });
  
    component.onSubmit();
  
    expect(propertyServiceMock.createProperty).toHaveBeenCalledWith({
      addressId: '1',
      address: {
        street: 'Test Street',
        city: 'Test City',
        state: 'Test State',
        postalCode: '12345',
        country: 'Test Country',
        additionalInfo: '',
      },
      imageId: '10',
      userId: '5',
      type: 'Apartment',
      features: {
        Garden: 1, // 1 pentru true
        Pool: 0,   // 0 pentru false
      },
    });
  
    expect(routerMock.navigate).toHaveBeenCalledWith(['/properties']);
  });
  

  it('should not submit the form if invalid', () => {
    component.propertyForm.patchValue({
      addressId: '',
      address: {
        street: '',
        city: '',
        state: '',
        postalCode: '',
        country: '',
        additionalInfo: '',
      },
      imageId: '',
      userId: '',
      type: '',
      features: {},
    });

    component.onSubmit();

    expect(propertyServiceMock.createProperty).not.toHaveBeenCalled();
    expect(routerMock.navigate).not.toHaveBeenCalled();
  });

  it('should handle errors when submitting the form', () => {
    propertyServiceMock.createProperty.and.returnValue(throwError('Error'));

    component.propertyForm.patchValue({
      addressId: '1',
      address: {
        street: 'Test Street',
        city: 'Test City',
        state: 'Test State',
        postalCode: '12345',
        country: 'Test Country',
        additionalInfo: '',
      },
      imageId: '10',
      userId: '5',
      type: 'Apartment',
      features: {
        Garden: 0,
        Pool: 1,
      },
    });

    component.onSubmit();

    expect(routerMock.navigate).not.toHaveBeenCalled();
  });

  it('should correctly convert features to numbers', () => {
    const features = { Garden: true, Pool: false, Balcony: true };
    const converted = component['convertFeaturesToNumbers'](features);
    expect(converted).toEqual({ Garden: 1, Pool: 0, Balcony: 1 });
  });
});
