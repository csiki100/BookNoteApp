import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Book } from '../../_models/book';
import { BookService } from '../../_services/book.service';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';


/**
 * @description Component that shows the Books of a User
 */
@Component({
  selector: 'app-member-books',
  templateUrl: './member-books.component.html',
  styleUrls: ['./member-books.component.css']
})
export class MemberBooksComponent implements OnInit {
  
  /**
   * @description Books of the User
   */
  books: Book[];

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private authService: AuthService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.books = data.books;
    });
  }

  /**
   * @description Function that updates the page when a Book is deleted
   */
  onDelete() {
    this.updatePage();
  }

  /**
   * @description Function that updates the page when a new Book is added
   */
  onAdd() {
    this.updatePage();
  }

  /**
   * @description Function that updates the page when a Book is edited
   */
  onEdit() {
    this.updatePage();
  }


  /**
   * @description Function that updates the page
   */
  updatePage() {
    this.bookService
      .getBooksForUser(this.authService.decodedToken.nameid)
      .subscribe(
        (books: Book[]) => {
          this.books = books;
        },
        error => {
          // Showing error message
          this.alertify.error(error);
        }
      );
  }
}
