import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { AddBookComponent } from '../book-add-modal/add-book.component';


/**
 * @description Component that shows a Card with a Button on it.
 * The User can add a new Book to his/her library by pressing the Button.
 */
@Component({
  selector: 'app-add-book-card',
  templateUrl: './add-book-card.component.html',
  styleUrls: ['./add-book-card.component.css']
})
export class AddBookCardComponent implements OnInit {
  /**
   * @description EventEmitter that emits an event when a new Book is added
   */
  @Output() added = new EventEmitter<void>();

  /**
   * @description Variable that represents the Modal
   */
  bsModalRef: BsModalRef;

  constructor(private modalService: BsModalService) {}

  ngOnInit() {}

  /**
   * @description Function that shows a Modal,
   * where the User can add a new Book to his/her library
   */
  openBookAddModal() {
    const initialState = { added: this.added };
    this.bsModalRef = this.modalService.show(AddBookComponent, {
      initialState
    });
  }
}
