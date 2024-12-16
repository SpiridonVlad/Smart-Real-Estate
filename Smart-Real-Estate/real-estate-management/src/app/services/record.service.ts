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

  getPaginatedRecords(page: number, pageSize: number): Observable<{ data: Record[]}> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

      console.log('params', params);

    return this.http.get<{ data: Record[]}>(`${this.apiUrl}/paginated`, { params }).pipe(
      catchError((error) => {
        console.error('Error fetching paginated records:', error);
        return throwError(error);
      })
    );
  }
}
