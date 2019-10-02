import {Routes} from '@angular/router';
import { HomeComponent } from './common/home/home.component';
import { MemberBooksComponent } from './member/member-books/member-books.component';
import { NonMemberGuard } from './_guards/non-member.guard';
import { UserbooksResolver } from './_resolvers/userbooks.resolver';
import { AddBookComponent } from './book/book-add-modal/add-book.component';
import { BookDetailComponent } from './book/book-detail/book-detail.component';
import { BookDetailResolver } from './_resolvers/bookdetail.resolver';
import { MemberGuard } from './_guards/member.guard';


export const appRoutes: Routes = [
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [MemberGuard],
    component: HomeComponent,
  },
         {
           path: '',
           runGuardsAndResolvers: 'always',
           canActivate: [NonMemberGuard],
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
