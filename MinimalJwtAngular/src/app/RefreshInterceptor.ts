import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpResponse, HttpRequest, HttpHandler, HttpErrorResponse } from '@angular/common/http'
import { Observable } from 'rxjs';
import { map, filter, catchError } from 'rxjs/operators';
import { StorageService } from 'src/app/services/storage.service';
import { AuthService } from './services/auth.service';
import { waitForAsync } from '@angular/core/testing';

@Injectable()
export class RefreshInterceptor implements HttpInterceptor {

  constructor(private storageService: StorageService, private authService: AuthService) { };

 intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      catchError(async error => {
        if (error instanceof HttpErrorResponse && !httpRequest.url.includes('auth/signin') && (error.status === 401) && this.storageService.getRefreshToken()) {
          let refresh = await this.authService.tryRefreshingTokens()
          if (refresh) {

          }
          }
        }
      )
    );
  }
}
