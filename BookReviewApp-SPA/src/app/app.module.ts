import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { ModalModule } from 'ngx-bootstrap/modal';

import { AppComponent } from './app.component';
import { NavComponent } from './member/member-nav/nav.component';
import { RegisterComponent } from './common/register/register.component';
import { HomeComponent } from './common/home/home.component';
import { LoginComponent } from './common/login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { MemberBooksComponent } from './member/member-books/member-books.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { UserbooksResolver } from './_resolvers/userbooks.resolver';
import { AuthService } from './_services/auth.service';
import { BookService } from './_services/book.service';
import { BookCardComponent } from './book/book-card/book-card.component';
import { AddBookCardComponent } from './book/book-add-card/add-book-card.component';
import { AddBookComponent } from './book/book-add-modal/add-book.component';
import { CommonModule } from '@angular/common';
import { BookDetailComponent } from './book/book-detail/book-detail.component';
import { BookDetailResolver } from './_resolvers/bookdetail.resolver';
import { CollapseModule } from 'ngx-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ChapterEditComponent } from './chapter/chapter-edit/chapter-edit.component';
import { BookEditModalComponent } from './book/book-edit-modal/book-edit-modal.component';
import { ChapterAddComponent } from './chapter/chapter-add/chapter-add.component';
import { AlertifyService } from './_services/alertify.service';
import { ErrorInterceptorProvider } from './_services/error.interceptor';


export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    RegisterComponent,
    HomeComponent,
    LoginComponent,
    MemberBooksComponent,
    BookCardComponent,
    BookCardComponent,
    AddBookCardComponent,
    AddBookComponent,
    BookDetailComponent,
    ChapterEditComponent,
    BookEditModalComponent,
    ChapterAddComponent
  ],
  imports: [
    ModalModule.forRoot(),
    BrowserModule,
    CommonModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    ReactiveFormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ["localhost:5000"],
        blacklistedRoutes: ["localhost:5000/api/auth"]
      }
    })
  ],
  entryComponents: [AddBookComponent, BookEditModalComponent],
  providers:
  [
    AlertifyService,
    AuthService,
    BookService,
    UserbooksResolver,
    BookDetailResolver,
    ErrorInterceptorProvider
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
