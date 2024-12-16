import { Component, OnInit } from '@angular/core';
import { RecordService } from '../../services/record.service';
import { Record } from '../../models/record.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-record-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './record-list.component.html',
  styleUrls: ['./record-list.component.css'],
})
export class RecordListComponent implements OnInit {
  records: Record[] = [];
  page: number = 1;
  pageSize: number = 5;
  pages: number[] = [1, 2, 3, 4, 5];
  pageSizes: number[] = [5, 10, 15];
  showFilterPopup: boolean = false;
  pTypes: string[] = ['Apartment', 'Office', 'Studio', 'CommercialSpace', 'House', 'Garage'];
  cities: string[] = ['New York', 'Los Angeles', 'Chicago', 'Houston', 'Phoenix', 'Philadelphia', 'San Antonio', 'San Diego', 'Dallas', 'San Jose'];
  filter: { 
    pType: string; 
    city: string ; 
    minPrice: number; 
    maxPrice: number;
    minPublicationDate?: string | null;
    maxPublicationDate?: string | null;
    descriptionContains?: string | null;
    propertyMinFeatures?: { [key: string]: number } | null;
    propertyMaxFeatures?: { [key: string]: number } | null;
    listingMinFeatures?: { [key: string]: number } | null;
  } = this.createDefaultFilter();

  constructor(private recordService: RecordService) {}

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
      },
      (error) => {
        console.error('Error loading records:', error);
      }
    );
  }
  

  getFeatures(features: { [key: string]: number }): { key: string; value: number }[] {
    return Object.entries(features).map(([key, value]) => ({ key, value }));
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
}
