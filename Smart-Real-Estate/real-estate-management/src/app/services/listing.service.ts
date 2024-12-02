import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Listing } from '../models/listing.model';

@Injectable({
  providedIn: 'root',
})
export class ListingService {
  private apiUrl = 'https://localhost:7117/api/v1/Listing';

  constructor(private http: HttpClient) {}

  public getPaginatedListings(page: number, pageSize: number): Observable<Listing[]> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<Listing[]>(this.apiUrl, { params }).pipe(
      catchError((error) => {
        console.error('Error fetching paginated listings:', error);
        return throwError(error);
      })
    );
  }

  public createListing(listing: Listing): Observable<any> {
    return this.http.post<Listing>(this.apiUrl, listing).pipe(
      catchError((error) => {
        console.error('Error creating listing:', error);
        return throwError(error);
      })
    );
  }

  public getListingById(id: string): Observable<Listing> {
    return this.http.get<Listing>(`${this.apiUrl}/${id}`).pipe(
      catchError((error) => {
        console.error('Error fetching listing by ID:', error);
        return throwError(error);
      })
    );
  }

  public updateListing(id: string, listing: Listing): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, listing).pipe(
      catchError((error) => {
        console.error('Error updating listing:', error);
        return throwError(error);
      })
    );
  }
}
