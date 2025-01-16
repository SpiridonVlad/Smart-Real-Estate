import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AiService {
  private apiUrl = 'https://localhost:7117/api/v1/PropertyPricePrediction/predict';

  constructor(private http: HttpClient) {}

  getPrediction(data: any): Observable<number> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'accept': 'text/plain',
    });

    return this.http.post<number>(this.apiUrl, data, { headers });
  }
}