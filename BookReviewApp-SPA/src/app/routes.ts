import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberBooksComponent } from './member-books/member-books.component';
import { AuthGuard } from './_guards/auth.guard';
import { UserbooksResolver } from './_resolvers/userbooks.resolver';
import { AddBookComponent } from './add-book/add-book.component';


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
               path: 'add',
               component: AddBookComponent
             }
           ]
         },
         { path: '**', redirectTo: '', pathMatch: 'full' }
       ];
