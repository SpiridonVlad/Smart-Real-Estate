import { Component, OnInit } from '@angular/core';
import { RecordService } from '../../services/record.service';
import { PropertyType, Record } from '../../models/record.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from "../header/header.component";
import { FooterComponent } from "../footer/footer.component";
import { Router } from '@angular/router';


@Component({
  selector: 'app-record-list',
  imports: [CommonModule, FormsModule, HeaderComponent, FooterComponent],
  templateUrl: './record-list.component.html',
  styleUrls: ['./record-list.component.css'],
})
export class RecordListComponent implements OnInit {
  records: Record[] = [];
  compareList: string[] = [];
  page: number = 1;
  pageSize: number = 5;
  pages: number[] = [1, 2, 3, 4, 5];
  pageSizes: number[] = [5, 10, 15];
  showFilterPopup: boolean = false;
  pTypes: string[] = Object.values(PropertyType);
  cities: string[] = ['New York', 'Los Angeles', 'Chicago', 'Houston', 'Phoenix', 'Philadelphia', 'San Antonio', 'San Diego', 'Dallas', 'San Jose'];
  filter: {
    pType: string;
    city: string;
    minPrice: number;
    maxPrice: number;
    minPublicationDate?: string | null;
    maxPublicationDate?: string | null;
    descriptionContains?: string | null;
    propertyMinFeatures?: { [key: string]: number } | null;
    propertyMaxFeatures?: { [key: string]: number } | null;
    listingMinFeatures?: { [key: string]: number } | null;
  } = this.createDefaultFilter();
  exists: boolean = false;
  constructor(private router: Router, private recordService: RecordService) {}

  ngOnInit(): void {
    this.loadRecords();
  }

  createDefaultFilter(): any {
    return {
      pType: '',
      city: '',
      minPrice: 0,
      maxPrice: 1000000,
      minPublicationDate: null,
      maxPublicationDate: null,
      descriptionContains: null,
      propertyMinFeatures: {
        Garden: 0,
        Garage: 0,
        Pool: 0,
        Balcony: 0,
        Rooms: 0,
        Surface: 0,
        Floor: 0,
        Year: 0,
        HeatingUnit: 0,
        AirConditioning: 0,
        Elevator: 0,
        Furnished: 0,
        Parking: 0,
        Storage: 0,
        Basement: 0,
        Attic: 0,
        Alarm: 0,
        Intercom: 0,
        VideoSurveillance: 0,
        FireAlarm: 0
      },
      propertyMaxFeatures: {
        Garden: 0,
        Garage: 0,
        Pool: 0,
        Balcony: 0,
        Rooms: 0,
        Surface: 0,
        Floor: 0,
        Year: 0,
        HeatingUnit: 0,
        AirConditioning: 0,
        Elevator: 0,
        Furnished: 0,
        Parking: 0,
        Storage: 0,
        Basement: 0,
        Attic: 0,
        Alarm: 0,
        Intercom: 0,
        VideoSurveillance: 0,
        FireAlarm: 0
      },
      listingMinFeatures: {
        IsSold: 0,
        IsHighlighted: 0,
        IsDeleted: 0
      }
    };
  }


  loadRecords(): void {
    this.recordService.getPaginatedRecords(this.page, this.pageSize, {
      pType: this.filter.pType,
      city: this.filter.city,
      minPrice: this.filter.minPrice,
      maxPrice: this.filter.maxPrice,
      minPublicationDate: this.filter.minPublicationDate,
      maxPublicationDate: this.filter.maxPublicationDate,
      descriptionContains: this.filter.descriptionContains,
      propertyMinFeatures: this.filter.propertyMinFeatures,
      propertyMaxFeatures: this.filter.propertyMaxFeatures,
      listingMinFeatures: this.filter.listingMinFeatures
    }).subscribe(
      (response: any) => {
        this.records = response.data;
        console.log('Records loaded:', this.records);
        this.exists = this.records.length > 0;
        console.log('Exists:', this.exists);
      },
      (error) => {
        console.error('Error loading records:', error);
      }
    );
  }


  getFeatures(features: { [key: string]: number } | null | undefined): { key: string; value: number }[] {
    // Ensure the input is a valid object
    if (!features || typeof features !== 'object') {
      return [];
    }
    // Map features to the required format
    return Object.entries(features).map(([key, value]) => ({ key, value }));
  }

  getListingFeatures(features: { [key: string]: number } | null | undefined): { key: string; value: number }[] {
    // Ensure the input is a valid object
    if (!features || typeof features !== 'object') {
      return [];
    }
    // Filter features to only include those with a value of 1
    return Object.entries(features)
      .map(([key, value]) => ({ key, value }));
  }


  toggleFilterPopup(): void {
    this.showFilterPopup = !this.showFilterPopup;
  }

  applyFilters(): void {
    if (this.filter.propertyMinFeatures) {
      for (const key in this.filter.propertyMinFeatures) {
        if (isNaN(this.filter.propertyMinFeatures[key])) {
          console.error(`Invalid value for ${key} in propertyMinFeatures`);
          return;
        }
      }
    }
    this.loadRecords();
    this.showFilterPopup = false;
  }


  nextPage(): void {
    if (this.page < this.pages.length) {
      this.page++;
      this.loadRecords();
    }
  }
  previousPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadRecords();
    }
  }

  getPropertyType(type: number): string {
    const propertyTypeMapping: { [key: number]: string } = {
      0: 'Apartment',
      1: 'Office',
      2: 'Studio',
      3: 'CommercialSpace',
      4: 'House',
      5: 'Garage',
    };
    return propertyTypeMapping[type] || 'Unknown';
  }

  getUserType(type: number): string {
    const userTypeMapping: { [key: number]: string } = {
      0: 'LegalEntity',
      1: 'Individual',
      2: 'Admin',
    };
    return userTypeMapping[type] || 'Unknown';
  }

  navigateToListing(listingId: string): void {
    this.router.navigate([`/records/${listingId}`]);
  }

  addToCompare(propertyId: string): void {
    const index = this.compareList.indexOf(propertyId);
    if (index === -1) {
      if (this.compareList.length < 2) {
        this.compareList.push(propertyId);
        console.log('Added to compare:', propertyId);
      } else {
        console.log('You can only compare up to 2 properties.');
      }
    } else {
      this.compareList.splice(index, 1);
      console.log('Removed from compare:', propertyId);
    }
  }

  isInCompareList(propertyId: string): boolean {
    return this.compareList.includes(propertyId);
  }

  navigateToCompare(): void {
    if (this.compareList.length === 2) {
      const [initial, secondary] = this.compareList;
      this.router.navigate(['/compare-properties'], { queryParams: { Initial: initial, Secondary: secondary } });
    } else {
      console.log('Please select exactly 2 properties to compare.');
    }
  }

  blockButtonIfNotInCompareList(id: string): boolean {
    return !this.compareList.includes(id) && this.compareList.length === 2;
  }


}
