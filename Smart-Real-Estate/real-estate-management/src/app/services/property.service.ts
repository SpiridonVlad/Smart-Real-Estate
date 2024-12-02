import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Property } from '../models/property.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private apiUrl = 'your-api-endpoint'; // Replace with your API endpoint

  constructor(private http: HttpClient) {}

  getPaginatedProperties(page: number, pageSize: number): Observable<Property[]> {
    return this.http.get<Property[]>(`${this.apiUrl}/properties?page=${page}&pageSize=${pageSize}`);
  }
}
