import { ComponentFixture, TestBed } from "@angular/core/testing";

import { ListingUpdateComponent } from "./listing-update.component";

describe("ListingUpdateComponent", () => {
  let component: ListingUpdateComponent;
  let fixture: ComponentFixture<ListingUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingUpdateComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ListingUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});