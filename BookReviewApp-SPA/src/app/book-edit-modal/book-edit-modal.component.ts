import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { BookService } from '../_services/book.service';
import { BsModalRef } from "ngx-bootstrap";
import { Book } from '../_models/book';

@Component({
  selector: "app-book-edit-modal",
  templateUrl: "./book-edit-modal.component.html",
  styleUrls: ["./book-edit-modal.component.css"]
})
export class BookEditModalComponent implements OnInit {
  bookInfo: Book;
  fileToUpload: File = null;
  nameToUpload: string = null;

  constructor(
    private authService: AuthService,
    private bookService: BookService,
    public bsModalRef: BsModalRef
  ) {}

  ngOnInit() {
    this.nameToUpload = this.bookInfo.name;
  }

  fileChange(event) {
    this.fileToUpload = event.target.files[0];
  }

  editBook() {
    this.bookService
      .editBook(
        this.authService.decodedToken.nameid,
        this.bookInfo.id,
        this.nameToUpload,
        this.fileToUpload
      )
      .subscribe(
        (returnedBook: Book) => {
          this.bookInfo.picture = returnedBook.picture;
          this.bookInfo.name = returnedBook.name;
          debugger;
          this.bsModalRef.hide();
        },
        error => {
          console.log(error);
        }
      );
  }
}
