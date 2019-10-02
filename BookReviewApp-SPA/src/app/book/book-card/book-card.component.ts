import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Book } from '../../_models/book';
import { BookService } from '../../_services/book.service';
import { AuthService } from '../../_services/auth.service';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { BookEditModalComponent } from '../book-edit-modal/book-edit-modal.component';
import { AlertifyService } from '../../_services/alertify.service';



/**
 * @description Component that shows a card with a Book's info on it.
 */
@Component({
  selector: 'app-book-card',
  templateUrl: './book-card.component.html',
  styleUrls: ['./book-card.component.css']
})
export class BookCardComponent implements OnInit {
  /**
   * @description Variable that stores the Book's data
   */
  @Input() book: Book;

  /**
   * @description EventEmitter that fires and event when the Book is edited
   */
  @Output() edited = new EventEmitter<boolean>();

  /**
   * @description EventEmitter that fires and event when the Book is deleted
   */
  @Output() deleted = new EventEmitter<boolean>();

  /**
   * @description Variable that represents the Modal
   */
  bsModalRef: BsModalRef;

  constructor(
    private bookService: BookService,
    private authService: AuthService,
    private router: Router,
    private modalService: BsModalService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}

  /**
   * @description Function that opens a BookEditModal  where the User can edit the Book
   */
  openBookEditModal() {
    const initialState = { bookInfo: this.book };
    this.bsModalRef = this.modalService.show(BookEditModalComponent, {
      initialState
    });
  }

  /**
   * @description Function that deletes the Book
   */
  deleteBook() {
    // Showing confirmation window
    this.alertify.confirm('Are you sure you want to delete the book?', () => {
      this.bookService
        .deleteBook(this.authService.decodedToken.nameid, this.book.id)
        .subscribe({
          next: () => {
            // Showing success message
            this.alertify.success('Book Deleted Successfully');
            // Firing event
            this.deleted.emit(true);
          },
          error: error => {
            // Showing error message
            this.alertify.error(error);
          }
        });
    });
  }
}
