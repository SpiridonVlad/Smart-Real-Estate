import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log('AuthInterceptor: intercepting request'); // Log to verify interceptor is called

    const token = this.authService.getToken();
    let cloned = req;

    if (token) {
      cloned = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
    }

    // Log the full request to check if the token is there
    console.log('HTTP Request:', cloned);
    console.log('Headers:', cloned.headers);

    return next.handle(cloned);
  }
}