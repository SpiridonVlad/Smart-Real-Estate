import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Property } from '../../models/property.model';
import { PropertyService } from '../../services/property.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-property-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.css'] // Ensure this line is commented out or removed
})
export class PropertyListComponent implements OnInit {
  properties: Property[] = [];
  page: number = 1;
  pageSize: number = 5;
  pages: number[] = [1, 2, 3, 4, 5];
  pageSizes: number[] = [5, 10, 15, 20];

  constructor(private propertyService: PropertyService, private router: Router) { }

  ngOnInit(): void {
    this.loadProperties();
  }

  loadProperties(): void {
    this.propertyService.getPaginatedProperties(this.page, this.pageSize).subscribe(
      (response: any) => {
        this.properties = response.data;
      },
      (error) => {
        console.error("Error loading properties:", error);
      }
    );
  }

  getNonZeroFeatures(features: { [key: string]: number }): { key: string; value: number }[] {
    return Object.entries(features)
      .filter(([key, value]) => value !== 0) // Filtrează doar valorile diferite de 0
      .map(([key, value]) => ({ key, value })); // Transformă într-un array de obiecte
  }

  navigateToCreate(): void {
    this.router.navigate(['/properties/create']);
  }

  navigateToUpdate(propertyId: string): void {
    this.router.navigate(['/properties/update', propertyId]);
  }

  deleteProperty(propertyId: string): void {
    if (confirm("Are you sure you want to delete this property?")) {
      this.propertyService.deleteProperty(propertyId).subscribe(
        () => {
          this.loadProperties(); // Reload the property list after deletion
        },
        (error) => {
          console.error("Error deleting property:", error);
        }
      );
    }
  }
}