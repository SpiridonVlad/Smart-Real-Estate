import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable,throwError } from 'rxjs';
import { User } from '../models/user.model';
import { catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7117/api/v1/User';

  constructor(private http: HttpClient) { }

  public getPaginatedUsers(page: number, pageSize: number): Observable<{ data: User[] }> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ data: User[] }>(this.apiUrl, { params }).pipe(
      catchError(error => {
        console.error('Error fetching paginated users:', error);
        return throwError(error);
      })
    );
  }

  public createUser(user: User): Observable<any> {
    return this.http.post<User>(this.apiUrl, user);
  }
  public getUserById(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  public updateUser(id: string, user: User): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, user);
  }
  public deleteUser(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}