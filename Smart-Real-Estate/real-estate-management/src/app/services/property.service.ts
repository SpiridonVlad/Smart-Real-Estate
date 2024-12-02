import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Property } from '../models/property.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private apiUrl = 'https://localhost:7117/api/v1/Property'; // Replace with your API endpoint

  constructor(private http: HttpClient) {}

  getProperties(page: number, pageSize: number): Observable<Property[]> {
    return this.http.get<Property[]>(`${this.apiUrl}/properties?page=${page}&pageSize=${pageSize}`);
  }
}
