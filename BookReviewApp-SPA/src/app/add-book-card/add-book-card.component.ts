import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { AddBookComponent } from '../add-book/add-book.component';

@Component({
  selector: "app-add-book-card",
  templateUrl: "./add-book-card.component.html",
  styleUrls: ["./add-book-card.component.css"]
})
export class AddBookCardComponent implements OnInit {
  bsModalRef: BsModalRef;
  constructor(private modalService: BsModalService) {}

  ngOnInit() {}

  openAddBook() {
    this.bsModalRef = this.modalService.show(AddBookComponent);
  }
}
