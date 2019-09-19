import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { BookEditModalComponent } from '../book-edit-modal/book-edit-modal.component';

@Component({
  selector: "app-book-card",
  templateUrl: "./book-card.component.html",
  styleUrls: ["./book-card.component.css"]
})
export class BookCardComponent implements OnInit {
  @Input() book: Book;
  @Output() edited = new EventEmitter<boolean>();
  @Output() deleted = new EventEmitter<boolean>();
  bsModalRef: BsModalRef;
  constructor(
    private bookService: BookService,
    private authService: AuthService,
    private router: Router,
    private modalService: BsModalService
  ) {}

  ngOnInit() {}

  openBookEditModal() {
    this.modalService.onHide.subscribe(() => {
      //this.edited.emit(true);
    });
    const initialState = { bookInfo: this.book };
    this.bsModalRef = this.modalService.show(BookEditModalComponent, { initialState });
  }

  deleteBook() {
    this.bookService
      .deleteBook(this.authService.decodedToken.nameid, this.book.id)
      .subscribe({
        next: () => {
          this.deleted.emit(true);
        },
        error: error => {
          console.log(error);
        }
      });
  }
}
