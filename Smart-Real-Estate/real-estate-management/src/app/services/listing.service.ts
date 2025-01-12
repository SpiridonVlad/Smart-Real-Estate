import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Listing } from '../models/listing.model';

@Injectable({
  providedIn: 'root',
})
export class ListingService {
  private apiUrl = 'https://localhost:7117/api/v1/Listing';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  public getPaginatedListings(page: number, pageSize: number): Observable<{ data: Listing[] }> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    const headers = this.getAuthHeaders();
    console.log('headers', headers);
    return this.http.get<{ data: Listing[] }>(`${this.apiUrl}/paginated`, { params, headers }).pipe(
      catchError((error) => {
        console.error('Error fetching paginated listings:', error);
        return throwError(error);
      })
    );
  }

  public createListing(listingData: Listing): Observable<any> {
    const headers = this.getAuthHeaders();
    const requestData = {
      propertyId: listingData.propertyId,
      price: listingData.price,
      publicationDate: new Date().toISOString(),  // If you don't have the exact date, you can use the current date
      description: listingData.description,
      features: listingData.features  // Pass the features object directly

    };

    return this.http.post<Listing>(this.apiUrl, requestData, { headers }).pipe(
      catchError((error) => {
        console.error('Error creating listing:', error);
        return throwError(error);
      })
    );
  }

  public getListingById(id: string): Observable<{ data: Listing }> {
    const headers = this.getAuthHeaders();
    return this.http.get<{ data: Listing }>(`${this.apiUrl}/${id}`, { headers }).pipe(
      catchError((error) => {
        console.error('Error fetching listing by ID:', error);
        return throwError(error);
      })
    );
  }

  public updateListing(id: string, listing: Listing): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.put(`${this.apiUrl}/${id}`, listing, { headers }).pipe(
      catchError((error) => {
        console.error('Error updating listing:', error);
        return throwError(error);
      })
    );
  }

  public deleteListing(id: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete(`${this.apiUrl}/${id}`, { headers }).pipe(
      catchError((error) => {
        console.error('Error deleting listing:', error);
        return throwError(error);
      })
    );
  }
}
