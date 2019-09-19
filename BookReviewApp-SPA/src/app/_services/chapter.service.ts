import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: "root"
})
export class ChapterService {

  baseUrl = environment.apiUrl + "chapters/";
  
  constructor(private http: HttpClient) {}

  deleteChapter(userId: number, bookId: number, chapterId: number) {
    return this.http.delete(this.baseUrl + userId + '/' + bookId + '/' + chapterId);
  }

  getChaptersForBook(userId: number, bookId: number) {
    return this.http.get(this.baseUrl + userId + '/' + bookId);
  }
}
