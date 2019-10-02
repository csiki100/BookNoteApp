import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { BookService } from '../../_services/book.service';
import { BsModalRef } from 'ngx-bootstrap';
import { Book } from '../../_models/book';
import { AlertifyService } from '../../_services/alertify.service';



/**
 * @description Component that shows a Modal Form where the user can edit a Book
 */
@Component({
  selector: 'app-book-edit-modal',
  templateUrl: './book-edit-modal.component.html',
  styleUrls: ['./book-edit-modal.component.css']
})
export class BookEditModalComponent implements OnInit {
  /**
   * @description Variable that stores the initial data of the Book
   */
  bookInfo: Book;

  /**
   * @description Variable that stores the new Picture of the Book
   */
  fileToUpload: File = null;

  /**
   * @description Variable that stores the new Name of the Book
   */
  nameToUpload = '';

  /**
   * @description Variable that stores if the Edit Button is disabled
   */
  buttonDisabled = false;

  constructor(
    private authService: AuthService,
    private bookService: BookService,
    public bsModalRef: BsModalRef,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.nameToUpload = this.bookInfo.name;
  }

  /**
   * @description Function that sets the bookPicture variable
   * when the User uploads a new Picture
   * @param event data of the event
   */
  fileChange(event) {
    this.fileToUpload = event.target.files[0];
  }


  /**
   * @description Function that edits the Book
   */
  editBook() {
    // Disabling the Edit Button
    this.buttonDisabled = true;
    this.bookService
      .editBook(
        this.authService.decodedToken.nameid,
        this.bookInfo.id,
        this.nameToUpload,
        this.fileToUpload
      )
      .subscribe(
        // Updating the Book's data
        (returnedBook: Book) => {
          this.bookInfo.picture = returnedBook.picture;
          this.bookInfo.name = returnedBook.name;
          // Showing success message and hiding the modal
          this.alertify.success('Book Edited Successfully');
          this.bsModalRef.hide();
        },
        error => {
          // Showing error message and enabling the Edit button
          this.buttonDisabled = false;
          this.alertify.error(error);
        }
      );
  }
}
