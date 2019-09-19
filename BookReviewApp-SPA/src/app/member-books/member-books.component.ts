import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AddBookComponent } from '../add-book/add-book.component';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-member-books',
  templateUrl: './member-books.component.html',
  styleUrls: ['./member-books.component.css']
})
export class MemberBooksComponent implements OnInit {
  books: Book[];

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.books = data.books;
      console.log(this.books);
    });
  }

  onDelete() {
    this.updatePage();
  }

  onAdd() {
    this.updatePage();
  }
  onEdit() {
    this.updatePage();
  }

  updatePage() {
    this.bookService
      .getBooksForUser(this.authService.decodedToken.nameid)
      .subscribe({
        next: (data: Book[]) => {
          this.books = data;
        }
      });
  }
  // proba() {
  //   this.bookService.getBooksForUser(6).subscribe((books :Obs) => {
  //     console.log(books);
  //   });
  // }
}
