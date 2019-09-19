import { Component, OnInit } from "@angular/core";
import { BookService } from "../_services/book.service";
import { BsModalRef } from "ngx-bootstrap";
import { AuthService } from "../_services/auth.service";

@Component({
  selector: "app-add-book",
  templateUrl: "./add-book.component.html",
  styleUrls: ["./add-book.component.css"]
})
export class AddBookComponent implements OnInit {
  fileToUpload: File = null;

  constructor(
    private authService: AuthService,
    private bookService: BookService,
    public bsModalRef: BsModalRef
  ) {}

  bookInfo: any = {};

  ngOnInit() {}

  fileChange(event) {
    this.fileToUpload = event.target.files[0];
  }

  addBook() {
    this.bookService
      .addBook(
        this.authService.decodedToken.nameid,
        this.bookInfo.name,
        this.fileToUpload
      )
      .subscribe(
        data => {
          this.bsModalRef.hide();
        },
        error => {
          console.log(error);
        }
      );
  }
}
