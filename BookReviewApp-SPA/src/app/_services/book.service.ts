import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';


/**
 * Service that handles the manipulation and retrieval of Book data.
 */
@Injectable({
  providedIn: 'root'
})
export class BookService {
  baseUrl = environment.apiUrl + 'books/';

  constructor(private http: HttpClient) {}

  /**
   * @description Function that gets the Books For a User through a GET request to the API
   * @param id The Id of the User.
   * @return — An Observable of the response,
   * with the Books in the resoinse body as a JSON object.
   */
  getBooksForUser(id: number) {
    return this.http.get(this.baseUrl + id);
  }

  /**
   * Function that adds a new Book to a User's library through a POST request to the API
   * @param userId The Id of the User
   * @param name the Name of the Book
   * @param fileToUpload  the Picture of the Book
   * @return — An Observable of the response,
   * with the created Book in the response body as a JSON object.
   */
  addBook(userId: number, name?: string, fileToUpload?: File) {
    const formData: FormData = new FormData();
    formData.append('name', name);
    formData.append('file', fileToUpload);
    return this.http.post(this.baseUrl + userId, formData);
  }

  /**
   * Function that edits a Book through a POST request to the API
   * @param userId The Id of the User
   * @param bookId The Id of the Book
   * @param name The new Name of the Book
   * @param fileToUpload the new Picture of the Book
   * @return An Observable of the response, with the response body as a JSON object.
   */
  editBook(userId: number, bookId: number, name?: string, fileToUpload?: File) {
    const formData: FormData = new FormData();
    formData.append('name', name);
    formData.append('file', fileToUpload);
    return this.http.post(this.baseUrl + userId + '/' + bookId, formData);
  }

  /**
   * Function that deletes a Book through a DELETE request to the API
   * @param userId the User's ID
   * @param bookId the Book's ID
   * @returns An Observable of the response
   */
  deleteBook(userId: number, bookId: number) {
    return this.http.delete(this.baseUrl + userId + '/' + bookId);
  }
}
