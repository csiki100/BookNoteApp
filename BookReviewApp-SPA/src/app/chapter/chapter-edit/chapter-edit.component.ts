import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewChild,
  HostListener
} from '@angular/core';
import { Chapter } from '../../_models/chapter';
import { ChapterService } from '../../_services/chapter.service';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { NgForm } from '@angular/forms';


/**
 * @description Component that shows a dropdown where the User can edit a Chapter
 */
@Component({
  selector: 'app-chapter-edit',
  templateUrl: './chapter-edit.component.html',
  styleUrls: ['./chapter-edit.component.css']
})
export class ChapterEditComponent implements OnInit {
  /**
   * @description the data of the Chapter
   */
  @Input() chapter: Chapter;

  /**
   * EventEmitter that fires an event when the Chapter is deleted
   */
  @Output() deleted = new EventEmitter<number>();

  /**
   * @description Variable that stores if the dropdown is closed or not
   */
  isCollapsed = true;

  /**
   * @description Variable that stores the new Name of the Chapter
   */
  nameToBe: string;

  /**
   * @description Variable that stores the new Content of the Chapter
   */
  contentToBe: string;

  /**
   * the edit Form
   */
  @ViewChild('editForm', { static: false }) editForm: NgForm;

  /**
   * @description Shows a confirmation window when the User want to close the page
   * while there are unsaved changes in the Form
   */
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private chapterService: ChapterService,
    private authService: AuthService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    // Setting up the initial values
    this.nameToBe = this.chapter.chapterName;
    this.contentToBe = this.chapter.content;
  }

  /**
   * @description Function that edits the Chapter
   */
  saveChanges() {

    const chapterToSave: Chapter = Object.assign({}, this.chapter);
    chapterToSave.chapterName = this.nameToBe;
    chapterToSave.content = this.contentToBe;

    this.chapterService.editChapter(chapterToSave).subscribe(
      () => {
        // Showing success message and updating the CHapter
        this.alertify.success('Chapter Edited Successfully');
        this.chapter.chapterName = chapterToSave.chapterName;
        this.chapter.content = chapterToSave.content;
      },
      error => {
        // Showing error message
        this.alertify.error(error);
      }
    );
  }

  /**
   * @description Function that reverts unsaved changes
   */
  revertChanges() {
    this.nameToBe = this.chapter.chapterName;
    this.contentToBe = this.chapter.content;
  }

  /**
   * @description Function that deletes the Chapter
   */
  deleteChapter() {

    // Showing confirmation window
    this.alertify.confirm(
      'Are you sure you want to delete the chapter?',
      () => {
        this.chapterService
          .deleteChapter(
            this.authService.decodedToken.nameid,
            this.chapter.bookId,
            this.chapter.id
          )
          .subscribe(
            () => {
              // Showing success message and firing event
              this.alertify.success('Chapter Deleted Successfully');
              this.deleted.emit(this.chapter.id);
            },
            error => {
              // Showing error message
              this.alertify.error(error);
            }
          );
      }
    );
  }

  /**
   * @description Function that prevents the dropdown from closing
   * @param event data of the event
   */
  private stayOpen(event: MouseEvent) {
    if (!this.isCollapsed) {
      event.stopImmediatePropagation();
    }
  }
}
