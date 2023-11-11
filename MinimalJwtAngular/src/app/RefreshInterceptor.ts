import { Injectable } from '@angular/core';
import { from } from 'rxjs';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap, retryWhen, scan, delay } from 'rxjs/operators';
import { AuthService } from 'src/app/services/auth.service';
import { StorageService } from 'src/app/services/storage.service'

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private maxRetryAttempts = 3; // Set a maximum number of retry attempts

  constructor(private authService: AuthService, private storageService: StorageService) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          // Token expired, try refreshing
          return from(this.authService.tryRefreshingTokens()).pipe(
            switchMap((refreshSuccess) => {
              if (refreshSuccess) {
                // Retry the original request with the new token
                const clonedRequest = request.clone({
                  withCredentials: true, // Ensure credentials are sent with the request
                });
                return next.handle(clonedRequest);
              } else {
                // If refreshing fails or the new token is also expired, redirect to login or handle as needed
                this.storageService.clean();
                this.storageService.setValueLoggedIn(false);// Example: Logout user
                return throwError('Token refresh failed');
              }
            }),
            // Retry the refresh a limited number of times
            retryWhen(errors => errors.pipe(
              scan((retryCount, error) => {
                if (retryCount >= this.maxRetryAttempts) {
                  throw error; // Give up after max retry attempts
                }
                return retryCount + 1;
              }, 0),
              delay(1000) // Delay between retry attempts (adjust as needed)
            ))
          ) as Observable<HttpEvent<any>>; // Correct the return type here
        } else {
          // Pass through other errors
          return throwError(error);
        }
      })
    );
  }
}
