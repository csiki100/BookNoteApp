import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { ModalModule } from 'ngx-bootstrap/modal';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { MemberBooksComponent } from './member-books/member-books.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { UserbooksResolver } from './_resolvers/userbooks.resolver';
import { AuthService } from './_services/auth.service';
import { BookService } from './_services/book.service';
import { BookCardComponent } from './book-card/book-card.component';
import { AddBookCardComponent } from './add-book-card/add-book-card.component';
import { AddBookComponent } from './add-book/add-book.component';
import { CommonModule } from '@angular/common';
import { BookDetailComponent } from './book-detail/book-detail.component';
import { BookDetailResolver } from './_resolvers/bookdetail.resolver';
import { CollapseModule } from 'ngx-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ChapterEditComponent } from './chapter-edit/chapter-edit.component';
import { BookEditModalComponent } from './book-edit-modal/book-edit-modal.component';


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
    BookEditModalComponent
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
        tokenGetter: tokenGetter,
        whitelistedDomains: ["localhost:5000"],
        blacklistedRoutes: ["localhost:5000/api/auth"]
      }
    })
  ],
  entryComponents: [AddBookComponent,BookEditModalComponent],
  providers: [AuthService, BookService, UserbooksResolver, BookDetailResolver],
  bootstrap: [AppComponent]
})
export class AppModule {}
