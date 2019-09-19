import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Chapter } from '../_models/chapter';
import { ChapterService } from '../_services/chapter.service';
import { AuthService } from '../_services/auth.service';
import { nextTick } from 'q';

@Component({
  selector: "app-chapter-edit",
  templateUrl: "./chapter-edit.component.html",
  styleUrls: ["./chapter-edit.component.css"]
})
export class ChapterEditComponent implements OnInit {
  @Input() chapter: Chapter;
  @Output() toDelete = new EventEmitter<number>();
  isCollapsed = true;

  constructor(private chapterService: ChapterService, private authService: AuthService) {}

  ngOnInit() {}

  saveChanges() { }
  deleteChapter() {
    this.chapterService.deleteChapter(this.authService.decodedToken.nameid,
      this.chapter.bookId, this.chapter.id).subscribe(() => {
        this.toDelete.emit(this.chapter.id);
      }, error => {
        console.log(error);
      });
  }
}
