import { Component, OnInit, EventEmitter, Input, Output, ViewChild, HostListener } from '@angular/core';
import { Chapter } from '../../_models/chapter';
import { ChapterService } from '../../_services/chapter.service';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { NgForm } from '@angular/forms';

/**
 * @description Component that shows a dropdown where the User can add a new Chapter to a Book
 */
@Component({
  selector: 'app-chapter-add',
  templateUrl: './chapter-add.component.html',
  styleUrls: ['./chapter-add.component.css']
})
export class ChapterAddComponent implements OnInit {
  /**
   * @description the ID of the Book
   */
  @Input() bookId: number;

  /**
   * @description EventEmitter that fires an event when a new Chapter is added
   */
  @Output() added = new EventEmitter<void>();

  /**
   * @description Variable that stores if the dropdown is closed or not
   */
  isCollapsed = true;

  /**
   * @description Variable that stores the Name of the new Chapter
   */
  nameToBe: string;

  /**
   * @description Variable that stores the Content of the new Chapter
   */
  contentToBe: string;

  /**
   * the add Form
   */
  @ViewChild('addForm', { static: false }) addForm: NgForm;

  /**
   * @description Shows a confirmation window when the User want to close the page
   * while there are unsaved changes in the Form
   */
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.addForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private chapterService: ChapterService,
    private authService: AuthService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}

  /**
   * @description Function that adds the Chapter to the Book
   */
  addChapter() {
    const chapter: Chapter = {
      id: 0,
      userId: this.authService.decodedToken.nameid,
      bookId: this.bookId,
      chapterName: this.nameToBe,
      content: this.contentToBe
    };

    this.chapterService.addChapter(chapter).subscribe(
      () => {
        this.alertify.success('Chapter Added');
        this.added.emit();
        this.isCollapsed = true;
        this.nameToBe = '';
        this.contentToBe = '';
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  /**
   * @description Function that prevents the dropdown from closing
   * @param event data of the event
   */
  stayOpen(event: MouseEvent) {
    if (!this.isCollapsed) {
      event.stopImmediatePropagation();
    }
  }
}
