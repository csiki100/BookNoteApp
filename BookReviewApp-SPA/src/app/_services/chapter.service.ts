import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Chapter } from '../_models/chapter';

/**
 * Service that handles the manipulation and retrieval of Chapter data.
 */
@Injectable({
  providedIn: 'root'
})
export class ChapterService {
  baseUrl = environment.apiUrl + 'chapters/';

  constructor(private http: HttpClient) {}

  /**
   * @description Function that Adds a new Chapter to a Book thorugh a POST request to the API
   * @param chapter the Chapter Object that the User wants to Add
   * @return — An Observable of the response,
   *  with the created Chapter in the response body as a JSON object.
   */
  addChapter(chapter: Chapter) {
    return this.http.post(this.baseUrl, chapter);
  }

  /**
   * @description Function that deletes a Chapter thorugh a DELETE request to the API
   * @param userId The USer's ID
   * @param bookId The Book's ID
   * @param chapterId The Chapter's ID
   * @return An Observable of the response.
   */
  deleteChapter(userId: number, bookId: number, chapterId: number) {
    return this.http.delete(
      this.baseUrl + userId + '/' + bookId + '/' + chapterId
    );
  }

  /**
   * @description Function that retrieves the Chapeters for a Book through a GET request to the API
   * @param userId the User's ID
   * @param bookId the Book's ID
   * @returns An Observable of response, with the Chapters as a JSON object.
   */
  getChaptersForBook(userId: number, bookId: number) {
    return this.http.get(this.baseUrl + userId + '/' + bookId);
  }

  /**
   * @description Function that edits a Chapter through a POST request to the API
   * @param chapter the edited Chapter's data
   * @returns An Observable of the response, with the response body as a JSON object.
   */
  editChapter(chapter: Chapter) {
    return this.http.post(this.baseUrl + 'edit', chapter);
  }
}
