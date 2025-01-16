import { Injectable } from '@angular/core';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class JwtHelperService {
  getUserIdFromToken(): string | null {
    const token = localStorage.getItem('jwt'); // Get JWT from local storage
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        return decoded.userId; // Replace 'userId' with the correct key in your JWT payload
      } catch (error) {
        console.error('Failed to decode JWT:', error);
        return null;
      }
    }
    return null;
  }
}
