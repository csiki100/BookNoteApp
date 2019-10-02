import {
  HttpInterceptor,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpRequest,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';

/**
 * A Class that intercepts Http Error responses and converts them to the desired format
 */
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(errorResponse => {
        // status code: 401
        if (errorResponse.status === 401) {
          // if there is something more in the response
          if (errorResponse.error != null) {
            return throwError(errorResponse.error);
          }
          // if there is nothing in the response
          return throwError(errorResponse.statusText);
        }

        if (errorResponse instanceof HttpErrorResponse) {
          // if the server's custom error is in the response
          const applicationError = errorResponse.headers.get(
            'Application-Error'
          );
          if (applicationError) {
            return throwError(applicationError);
          }

          // if there are model state errors, or server error
          const serverError = errorResponse.error;
          let modelStateErrors = '';
          // Gathering all of the model state errors
          if (serverError.errors && typeof serverError.errors === 'object') {
            for (const key in serverError.errors) {
              if (serverError.errors[key]) {
                modelStateErrors += serverError.errors[key] + '\n';
              }
            }
          }
          return throwError(modelStateErrors || serverError || 'Server Error');
        }
      })
    );
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
