import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

/**
 * @description Class that ensures the data is present for the "/books/:id" root,
 * when the root is activated
 */
@Injectable()
export class BookDetailResolver implements Resolve<Book> {
  baseUrl = environment.apiUrl + 'books/';

  constructor(
    private bookService: BookService,
    private http: HttpClient,
    private router: Router,
    private alertify: AlertifyService,
    private authService: AuthService,
  ) {}
 /**
  * @description function that retrieves the necessary data to activate the route
  * @param route parameter that contains information about the activated route
  * @returns a Book Entity from the Server
  */
  resolve(route: ActivatedRouteSnapshot): Observable<Book> {
    return this.http.get<Book>(this.baseUrl + this.authService.decodedToken.nameid + '/' +
      route.params.id).pipe(
      catchError(error => {
        this.alertify.error(error);
        this.router.navigate(['/user']);
        return of(null);
      })
    );
  }
}
