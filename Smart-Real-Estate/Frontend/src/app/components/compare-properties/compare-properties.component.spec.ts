import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComparePropertiesComponent } from './compare-properties.component';

describe('ComparePropertiesComponent', () => {
  let component: ComparePropertiesComponent;
  let fixture: ComponentFixture<ComparePropertiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComparePropertiesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComparePropertiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
