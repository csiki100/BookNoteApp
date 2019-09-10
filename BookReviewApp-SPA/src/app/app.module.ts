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
      BookDetailComponent
   ],
   imports: [
      ModalModule.forRoot(),
      BrowserModule,
      CommonModule,
      HttpClientModule,
      FormsModule,
      RouterModule.forRoot(appRoutes),
      ReactiveFormsModule,
     JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes:['localhost:5000/api/auth']
         }
  ],
  entryComponents: [AddBookComponent],
  providers: [AuthService, BookService, UserbooksResolver],
  bootstrap: [AppComponent]
})
export class AppModule {}
