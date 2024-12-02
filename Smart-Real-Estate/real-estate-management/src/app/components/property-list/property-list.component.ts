import { Component, OnInit } from '@angular/core';
import { PropertyService } from '../../services/property.service';
import { Router } from '@angular/router';
import { Property } from '../../models/property.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-property-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.css']
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
      (response: any) => {
        this.properties = response.data;
      },
      (error) => {
        console.error('Error loading properties:', error);
      }
    );
  }

  navigateToCreate(): void {
    this.router.navigate(['/properties/create']);
  }
}
