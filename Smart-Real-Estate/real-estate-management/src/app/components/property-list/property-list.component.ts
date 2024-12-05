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

  constructor(private propertyService: PropertyService, private router: Router) { }

  ngOnInit(): void {
    this.loadProperties();
  }

  loadProperties(): void {
    this.propertyService.getPaginatedProperties(this.page, this.pageSize).subscribe(
      (response: Property[]) => {
        this.properties = response;
        console.log('Properties loaded:', this.properties); // Log properties
        this.properties.forEach(property => {
          this.propertyService.getAddressById(property.addressId).subscribe(
            address => {
              property.address = address;
              console.log(`Address for property ${property.id}:`, address); // Log address
            },
            error => {
              console.error('Error fetching address:', error);
            }
          );
        });
      },
      (error) => {
        console.error('Error loading properties:', error);
      }
    );
  }

  getFeatures(features: { [key: string]: number }): { key: string, value: number }[] {
    return Object.keys(features).map(key => ({ key, value: features[key] }));
  }

  navigateToCreate(): void {
    this.router.navigate(['/properties/create']);
  }

  navigateToUpdate(propertyId: string): void {
    this.router.navigate(['/properties/update', propertyId]);
  }
}