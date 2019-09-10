import { Component, OnInit } from '@angular/core';
import { BookService } from '../_services/book.service';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: "app-add-book",
  templateUrl: "./add-book.component.html",
  styleUrls: ["./add-book.component.css"]
})
export class AddBookComponent implements OnInit {
  fileToUpload: File = null;

  constructor(
    private bookService: BookService,
    public bsModalRef: BsModalRef
  ) {}

  bookInfo: any = {};

  ngOnInit() {}

  uploadFile(file: FileList) {
    this.fileToUpload = file.item(0);
  }

  saveFile() {
    this.bookService.addBook(this.fileToUpload).subscribe(
      data => {
        // do something, if upload success
      },
      error => {
        console.log(error);
      }
    );
  }
}
