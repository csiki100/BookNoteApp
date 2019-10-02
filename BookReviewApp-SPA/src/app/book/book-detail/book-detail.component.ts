import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from '../../_models/book';
import { BookService } from '../../_services/book.service';
import { AuthService } from '../../_services/auth.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { BookEditModalComponent } from '../book-edit-modal/book-edit-modal.component';
import { ChapterService } from '../../_services/chapter.service';
import { Chapter } from '../../_models/chapter';
import { AlertifyService } from '../../_services/alertify.service';

/**
 * @description Component that shows a Book's data and it's chapters
 */
@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css']
})
export class BookDetailComponent implements OnInit {
  /**
   * @description Variable that stores the Book's data
   */
  book: Book;

  /**
   * @description Variable that represents the Modal
   */
  bsModalRef: BsModalRef;

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private authService: AuthService,
    private router: Router,
    private modalService: BsModalService,
    private chapterService: ChapterService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.book = data.book;
    });
  }

  /**
   * @description Function that opens a BookEditModal where the User can edit the Book
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
            // Showing success message and navigating the User
            this.alertify.success('Book Deleted Successfully');
            this.router.navigate(['user']);
          },
          error: error => {
            // Showing error message
            this.alertify.error(error);
          }
        });
    });
  }

  /**
   * @description Function that gets called when a Chapter is deleted
   */
  chapterDeleted() {
    this.getChaptersForBook();
  }

  /**
   * @description Function that gets called when a new Chapter is added
   */
  chapterAdded() {
    this.getChaptersForBook();
  }

  /**
   * @description Function that retrieves the Chapters for the Book
   */
  getChaptersForBook() {
    this.chapterService
      .getChaptersForBook(this.authService.decodedToken.nameid, this.book.id)
      .subscribe(
        (chapters: Chapter[]) => {
          this.book.chapters = chapters;
        },
        error => {
          // Showing error message
          this.alertify.error(error);
        }
      );
  }
}
