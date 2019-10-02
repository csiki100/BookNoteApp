import { Component, OnInit, EventEmitter } from '@angular/core';
import { BookService } from '../../_services/book.service';
import { BsModalRef } from 'ngx-bootstrap';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';


/**
 * @description Component that shows a form where the User can create a new Book
 */
@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css']
})
export class AddBookComponent implements OnInit {
  /**
   * @description The new Book's Name
   */
  bookName = '';

  /**
   * @description the new Book's Picture
   */
  bookPicture: File = null;

  /**
   * @description Variable that stores if the Create Button is disabled
   */
  buttonDisabled = false;

  /**
   * @description EventEmitter that fires an event when a new Book is added
   */
  added: EventEmitter<void>;

  constructor(
    private authService: AuthService,
    private bookService: BookService,
    public bsModalRef: BsModalRef,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}

  /**
   * @description Function that sets the bookPicture variable
   * when the User uploads a new Picture
   * @param event data of the event
   */
  fileChange(event) {
    this.bookPicture = event.target.files[0];
  }

  /**
   * @description Function that adds a new Book to the User's library
   */
  addBook() {
    // Disabling the Create Button
    this.buttonDisabled = true;

    this.bookService
      .addBook(
        this.authService.decodedToken.nameid,
        this.bookName,
        this.bookPicture
      )
      .subscribe(
        () => {
          // Showing success message
          this.alertify.success('Book Created Successfully');
          // Firing event
          this.added.emit();
          // Hiding Modal
          this.bsModalRef.hide();
        },
        error => {
          // Showing error
          this.alertify.error(error);
          // Enabling the Create Button
          this.buttonDisabled = false;
        }
      );
  }
}
