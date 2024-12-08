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

  public getPaginatedListings(page: number, pageSize: number): Observable<{ data: Listing[]}> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ data: Listing[] }>(this.apiUrl, { params }).pipe(
      catchError((error) => {
        console.error('Error fetching paginated listings:', error);
        return throwError(error);
      })
    );
  }

  createListing(listingData: Listing): Observable<any> {
    // Asigură-te că structura JSON e corectă înainte de a trimite cererea
    const requestData = {
      propertyId: listingData.propertyId,
      userId: listingData.userId,
      price: listingData.price,
      publicationDate: new Date().toISOString(),  // Dacă nu ai data exactă, poți folosi data curentă
      description: listingData.description,
      features: {
        features: listingData.features  // Transmite obiectul features direct
      }
    };

    // Trimite cererea POST cu structura corectă
    return this.http.post<Listing>(this.apiUrl, requestData);
  }

  public getListingById(id: string): Observable<{data: Listing}> {
    return this.http.get<{ data: Listing}>(`${this.apiUrl}/${id}`).pipe(
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

  public deleteListing(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError((error) => {
        console.error('Error deleting listing:', error);
        return throwError(error);
      })
    );
  }
}
