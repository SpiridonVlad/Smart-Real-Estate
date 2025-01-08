import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Record } from '../models/record.model';

@Injectable({
  providedIn: 'root',
})
export class RecordService {
  private apiUrl = 'https://localhost:7117/api/v1/Record';

  constructor(private http: HttpClient) {}

  getPaginatedRecords(page: number, pageSize: number, filters: any): Observable<{ data: Record[] }> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
  
    if (filters.pType) {
      params = params.set('propertyType', filters.pType);
    }
    if (filters.minPrice) {
      params = params.set('minPrice', filters.minPrice.toString());
    }
    if (filters.maxPrice) {
      params = params.set('maxPrice', filters.maxPrice.toString());
    }
    if (filters.minPublicationDate) {
      params = params.set('minPublicationDate', filters.minPublicationDate);
    }
    if (filters.maxPublicationDate) {
      params = params.set('maxPublicationDate', filters.maxPublicationDate);
    }
    if (filters.descriptionContains) {
      params = params.set('descriptionContains', filters.descriptionContains);
    }
  
    // Funcție pentru aplatizarea obiectelor în parametri cheie-valoare
    const flattenObjectToParams = (obj: { [key: string]: number } | null, paramPrefix: string) => {
      if (obj) {
        Object.entries(obj).forEach(([key, value]) => {
          params = params.set(key, value.toString());
        });
      }
    };
  
    // Aplatizează `propertyMinFeatures`, `propertyMaxFeatures`, și `listingMinFeatures`
    flattenObjectToParams(filters.propertyMinFeatures, 'propertyMinFeatures');
    flattenObjectToParams(filters.propertyMaxFeatures, 'propertyMaxFeatures');
    flattenObjectToParams(filters.listingMinFeatures, 'listingMinFeatures');
  
    return this.http.get<{ data: Record[] }>(`${this.apiUrl}/Paginated`, { params }).pipe(
      catchError((error) => {
        console.error('Error fetching paginated records:', error);
        return throwError(error);
      })
    );
  }  
}
