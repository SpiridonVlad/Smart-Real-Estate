import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7117/api/v1/User';

  constructor(private http: HttpClient, private authService: AuthService) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getPaginatedUsers(page: number, pageSize: number, minRating?: number, maxRating?: number, verified?: boolean, type?: number, username?: string): Observable<{ data: User[] }> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    if (minRating !== undefined) {
      params = params.set('minRating', minRating.toString());
    }
    if (maxRating !== undefined) {
      params = params.set('maxRating', maxRating.toString());
    }
    if (verified !== undefined) {
      params = params.set('verified', verified.toString());
    }
    if (type) {
      params = params.set('type', type);
    }
    if (username) {
      params = params.set('username', username);
    }

    return this.http.get<{ data: User[] }>(`${this.apiUrl}/Paginated`, { params });
  }

  public createUser(user: User): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.post<User>(this.apiUrl, user, { headers });
  }

  public getUserById(id: string): Observable<{ data: User }> {
    const headers = this.getAuthHeaders();
    return this.http.get<{ data: User }>(`${this.apiUrl}/${id}`, { headers });
  }

  public updateUser(id: string, user: User): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.put(`${this.apiUrl}/${id}`, user, { headers });
  }

  public deleteUser(id: string): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete(`${this.apiUrl}/${id}`, { headers });
  }
}