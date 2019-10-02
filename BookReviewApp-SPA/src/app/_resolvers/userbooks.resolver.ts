import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';


/**
 * @description Class that ensures the data is present for the "/user" root,
 * when the root is activated
 */
@Injectable()
export class UserbooksResolver implements Resolve<Book[]> {
  constructor(
    private bookService: BookService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  /**
   * @description function that retrieves the necessary data to activate the route
   * @param route parameter that contains information about the activated route
   * @returns the Books of a User
   */
  resolve(route: ActivatedRouteSnapshot): Observable<Book[]> {
    return this.bookService
      .getBooksForUser(this.authService.decodedToken.nameid)
      .pipe(
        catchError(error => {
          this.alertify.error(error);
          return of(null);
        })
      );
  }
}
