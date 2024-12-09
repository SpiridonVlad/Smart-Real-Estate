import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Property } from '../models/property.model';

@Injectable({
  providedIn: 'root',
})
export class PropertyService {
  private apiUrl = 'https://localhost:7117/api/v1/Property';

  constructor(private http: HttpClient) {}

  public getPaginatedProperties(page: number, pageSize: number): Observable<{ data: Property[]}> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ data: Property[] }>(this.apiUrl, { params }).pipe(
      catchError((error) => {
        console.error('Error fetching paginated properties:', error);
        return throwError(error);
      })
    );
  }

  createProperty(propertyData: Property): Observable<any> {
    // Asigură-te că structura JSON e corectă înainte de a trimite cererea
    const requestData = {
      addressId: propertyData.addressId,
      userId: propertyData.userId,
      type: propertyData.type,
      features: {
        features: propertyData.features  // Transmite obiectul features direct
      }
    };

    // Trimite cererea POST cu structura corectă
    return this.http.post<Property>(this.apiUrl, requestData);
  }

  public getPropertyById(id: string): Observable<{data: Property}> {
    return this.http.get<{ data: Property}>(`${this.apiUrl}/${id}`).pipe(
      catchError((error) => {
        console.error('Error fetching property by ID:', error);
        return throwError(error);
      })
    );
  }

  public updateProperty(id: string, property: Property): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, property).pipe(
      catchError((error) => {
        console.error('Error updating property:', error);
        return throwError(error);
      })
    );
  }

  public deleteProperty(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError((error) => {
        console.error('Error deleting property:', error);
        return throwError(error);
      })
    );
  }
}

