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

  public getPaginatedUsers(page: number, pageSize: number): Observable<{ data: User[] }> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    const headers = this.getAuthHeaders();

    return this.http.get<{ data: User[] }>(`${this.apiUrl}/paginated`, { params, headers });
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