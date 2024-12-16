import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7117/api/v1/Authentication'; // Base API URL
  private tokenKey = 'authToken';

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { email, password }).pipe(
      tap(response => this.setToken(response.data)),
      catchError(this.handleError('login', []))
    );
  }

  register(username: string, password: string, email: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, { username, password, email, type: 0 }).pipe(
      catchError(this.handleError('register', []))
    );
  }
  verifyEmail(token: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Confirm`, { token }).pipe(
      catchError(this.handleError('verifyEmail', []))
    );
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
  printToken(): void {
    const token = this.getToken();
    console.log('Saved token:', token);
  }
}