import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Property } from '../models/property.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private apiUrl = 'https://localhost:7117/api/v1/Property'; // Replace with your API endpoint

  constructor(private http: HttpClient) { }

  public getPaginatedProperties(page: number, pageSize: number): Observable<Property[]> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ data: Property[] }>(this.apiUrl, { params }).pipe(
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

  public getAddressById(addressId: string): Observable<any> {
    const hardcodedAddresses: { [key: string]: { street: string; city: string; state: string; country: string } } = {
      '3fa85f64-5717-4562-b3fc-2c963f66afa6': { street: '123 Main St', city: 'Anytown', state: 'Anystate', country: 'Anycountry' },
      'e38b0929-0c06-4038-9a2c-343a482773db': { street: '456 Elm St', city: 'Othertown', state: 'Otherstate', country: 'Othercountry' },
      // Add more hardcoded addresses as needed
    };

    const address = hardcodedAddresses[addressId] || null;

    if (address) {
      return of(address);
    } else {
      console.error('Error fetching address: Address not found');
      return throwError('Address not found');
    }
  }

  public createProperty(property: Property): Observable<any> {
    return this.http.post<Property>(this.apiUrl, property);
  }
  public updateProperty(property: Property): Observable<any> {
    return this.http.put<Property>(`${this.apiUrl}/${property.id}`, property).pipe(
      catchError(error => {
        console.error('Error updating property:', error);
        return throwError(error);
      })
    );
  }
  public getPropertyById(id: string): Observable<Property> {
    return this.http.get<Property>(`${this.apiUrl}/${id}`).pipe(
      catchError(error => {
        console.error('Error fetching property:', error);
        return throwError(error);
      })
    );
  }
}