import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ListingCreateComponent } from './listing-create.component';
import { ListingService } from '../../services/listing.service';
import { Listing } from '../../models/listing.model';

describe('ListingCreateComponent', () => {
  let component: ListingCreateComponent;
  let fixture: ComponentFixture<ListingCreateComponent>;
  let service: ListingService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule
      ],
      declarations: [],
      providers: [ListingService]
    }).compileComponents();

    fixture = TestBed.createComponent(ListingCreateComponent);
    component = fixture.componentInstance;
    service = TestBed.inject(ListingService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
