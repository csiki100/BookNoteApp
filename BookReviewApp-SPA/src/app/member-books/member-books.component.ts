import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AddBookComponent } from '../add-book/add-book.component';

@Component({
  selector: "app-member-books",
  templateUrl: "./member-books.component.html",
  styleUrls: ["./member-books.component.css"]
})
export class MemberBooksComponent implements OnInit {
  books: Book[];

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.books = data["books"];
      console.log(this.books);
    });
  }

  // proba() {
  //   this.bookService.getBooksForUser(6).subscribe((books :Obs) => {
  //     console.log(books);
  //   });
  // }
}
