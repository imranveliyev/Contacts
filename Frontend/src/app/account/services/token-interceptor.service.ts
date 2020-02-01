import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { TokenStorageService } from './token-storage.service';
import { catchError, switchMap } from 'rxjs/operators';
import { AccountApiService } from './account-api.service';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {

  constructor(
    private tokenStorage: TokenStorageService,
    private accountApi: AccountApiService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let requestWithToken = this.addAccessToken(request);
    return next.handle(requestWithToken).pipe(
      catchError(error => {
        if (error.status == 401) return this.handleError(request, next);
        return throwError(error);
      })
    );
  }

  addAccessToken(request: HttpRequest<any>): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        'Authorization': `Bearer ${this.tokenStorage.accessToken}`
      }
    });
  }

  handleError(request: HttpRequest<any>, next: HttpHandler) {
    return this.accountApi.refresh(this.tokenStorage.refreshToken).pipe(
      switchMap(response => {
        this.tokenStorage.setTokens(response.accessToken, response.refreshToken);
        let authRequest = this.addAccessToken(request);
        return next.handle(authRequest);
      }));
  }
}
