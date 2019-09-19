import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';


@Injectable()
export class BookDetailResolver implements Resolve<Book> {

  baseUrl = environment.apiUrl + 'books/';

  constructor(private bookService: BookService, private http: HttpClient) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Book> {
    return this.http.get<Book>(this.baseUrl + 'book/' + route.params.id);
  }
}
