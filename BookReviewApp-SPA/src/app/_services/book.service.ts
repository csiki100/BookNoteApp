import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Book } from '../_models/book';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class BookService {
  baseUrl = environment.apiUrl + 'books/';

  constructor(private http: HttpClient) { }


  getBooksForUser(id: number) {
    const params = new HttpParams();
    params.append('pageSize', '1');
    return this.http.get(this.baseUrl + id, { observe: 'response', params }).pipe(
      map(
        response => {
          return response.body;
        }
      )
    );
  }
  addBook(userId: number, name: string, fileToUpload: File){
    const formData: FormData = new FormData();
    formData.append('name', name);
    formData.append('file', fileToUpload);
    return this.http.post(this.baseUrl + userId, formData);
  }

  editBook(userId: number, bookId: number, name?: string, fileToUpload?: File) {

    const formData: FormData = new FormData();
    formData.append("name", name);
    formData.append("file", fileToUpload);

    return this.http.post(this.baseUrl + userId + '/' + bookId, formData);
  }

  deleteBook(userId: number, bookId: number) {
    return this.http.delete(this.baseUrl + userId + '/' + bookId);
  }
}
