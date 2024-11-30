import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7117/api/v1/User';

  constructor(private http: HttpClient) { }

  public getPaginatedUsers(page: number, size: number): Observable<User[]> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('size', size.toString());

    return this.http.get<User[]>(this.apiUrl, { params });
  }
  public createUser(user: User): Observable<any> {
    return this.http.post<User>(this.apiUrl, user);
  }
}
