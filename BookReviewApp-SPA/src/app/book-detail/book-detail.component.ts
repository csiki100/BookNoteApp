import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from '../_models/book';
import { BookService } from '../_services/book.service';
import { AuthService } from '../_services/auth.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { BookEditModalComponent } from '../book-edit-modal/book-edit-modal.component';
import { ChapterService } from '../_services/chapter.service';
import { Chapter } from '../_models/chapter';

@Component({
  selector: "app-book-detail",
  templateUrl: "./book-detail.component.html",
  styleUrls: ["./book-detail.component.css"]
})
export class BookDetailComponent implements OnInit {
  book: Book;
  bsModalRef: BsModalRef;
  isCollapsed = false;
  
  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private authService: AuthService,
    private router: Router,
    private modalService: BsModalService,
    private chapterService: ChapterService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.book = data.book;
    });
  }

  openBookEditModal() {
    this.modalService.onHide.subscribe(() => {
    });
    const initialState = {bookInfo:this.book};
    this.bsModalRef = this.modalService.show(BookEditModalComponent, {
      initialState
    });
  }

  deleteBook() {
    this.bookService
      .deleteBook(this.authService.decodedToken.nameid, this.book.id)
      .subscribe({
        next: () => {
          this.router.navigate(["user"]);
        },
        error: error => {
          console.log(error);
        }
      });
  }

  deleteChapter() {
    this.getChaptersForBook();
  }

  getChaptersForBook() {
    this.chapterService.getChaptersForBook(this.authService.decodedToken.nameid, this.book.id)
      .subscribe((chapters: Chapter[]) => {
        this.book.chapters = chapters;
      }, error => {
          console.log(error);
      });
  }
}
