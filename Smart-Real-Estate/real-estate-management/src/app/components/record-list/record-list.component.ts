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
  filter: { pType: string; city: string ; minPrice: number; maxPrice: number} = { pType: '', city: '', minPrice: 0, maxPrice: 999999 };

  constructor(private recordService: RecordService) {}

  ngOnInit(): void {
    this.loadRecords();
  }

  loadRecords(): void {
    this.recordService.getPaginatedRecords(this.page, this.pageSize).subscribe(
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

  getPropertyType(type: string): string {
    const propertyTypes = [
      'Apartment',
      'Office',
      'Studio',
      'Commercial Space',
      'House',
      'Garage',
    ];
    return propertyTypes[parseInt(type)] || 'Unknown';
  }

  getNonZeroFeatures(features: { [key: string]: number }): { key: string; value: number }[] {
    return Object.entries(features)
      .map(([key, value]) => ({ key, value })) // Transformă într-un array de obiecte
      .filter(feature => feature.value !== 0); // Filtrează doar valorile diferite de 0
  }
}