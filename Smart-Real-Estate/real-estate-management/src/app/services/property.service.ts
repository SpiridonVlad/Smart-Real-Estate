import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Property } from '../models/property.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private apiUrl = 'https://localhost:7117/api/v1/Property'; // Replace with your API endpoint

  constructor(private http: HttpClient, private authService: AuthService) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  public getPaginatedProperties(page: number, pageSize: number): Observable<Property[]> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());


      const headers = this.getAuthHeaders();
    return this.http.get<{ data: Property[] }>(`${this.apiUrl}/paginated`, { params, headers }).pipe(
      map(response => response.data), // Extract the array of properties from the response
      catchError(error => {
        console.error('Error fetching paginated properties:', error);
        console.error('Error status:', error.status);
        console.error('Error message:', error.message);
        console.error('Error details:', error.error);
        return throwError(error);
      })
    );
  }

  public getPropertyById(id: string): Observable<{data: Property}> {
    const headers = this.getAuthHeaders();
    return this.http.get<{ data: Property}>(`${this.apiUrl}/${id}`, { headers }).pipe(
      catchError((error) => {
        console.error('Error fetching property by ID:', error);
        return throwError(error);
      })
    );
  }

  public createProperty(property: Property): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.post<Property>(this.apiUrl, property, { headers });
  }
  public updateProperty(property: Property): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.put<Property>(`${this.apiUrl}/${property.id}`, property, { headers }).pipe(
      catchError(error => {
        console.error('Error updating property:', error);
        return throwError(error);
      })
    );
  }

  public deleteProperty(id: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete(`${this.apiUrl}/${id}`, { headers }).pipe(
      catchError((error) => {
        console.error('Error deleting property:', error);
        return throwError(error);
      })
    );
  }
}