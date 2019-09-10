import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';



@Injectable()
export class UserbooksResolver implements Resolve<Book[]> {
  constructor(
    private bookService: BookService,
    private authService: AuthService,
    private router: Router
  ) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Book[]> {
        return this.bookService
          .getBooksForUser(this.authService.decodedToken.nameid)
          .pipe(
            catchError(error => {
              console.log(error);
              this.router.navigate(['/home']);
              return of(null);
            })
          );
  }
}
