import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberBooksComponent } from './member-books/member-books.component';
import { AuthGuard } from './_guards/auth.guard';
import { UserbooksResolver } from './_resolvers/userbooks.resolver';
import { AddBookComponent } from './add-book/add-book.component';
import { BookDetailComponent } from './book-detail/book-detail.component';
import { BookDetailResolver } from './_resolvers/bookdetail.resolver';


export const appRoutes: Routes = [
         { path: '', component: HomeComponent },
         {
           path: '',
           runGuardsAndResolvers: 'always',
        //    canActivate: [AuthGuard],
           children: [
             {
               path: 'user',
               component: MemberBooksComponent,
                resolve: { books: UserbooksResolver }
             },
             {
               path: 'book/:id',
               component: BookDetailComponent,
               resolve: {book: BookDetailResolver}
             }
           ]
         },
         { path: '**', redirectTo: '', pathMatch: 'full' }
       ];
