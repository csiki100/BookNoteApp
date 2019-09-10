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
  addBook(fileToUpload: File){
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.http.post(this.baseUrl, formData);
  }
}
