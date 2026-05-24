import { Routes } from '@angular/router';
import { Home } from './pages/home/home';

import { CategoryList } from './pages/categories/category-list/category-list';
import { CategoryForm } from './pages/categories/category-form/category-form';
import { CategoryDetail } from './pages/categories/category-detail/category-detail';

import { AuthorList } from './pages/authors/author-list/author-list';
import { AuthorForm } from './pages/authors/author-form/author-form';
import { AuthorDetail } from './pages/authors/author-detail/author-detail';

import { MemberList } from './pages/members/member-list/member-list';
import { MemberForm } from './pages/members/member-form/member-form';
import { MemberDetail } from './pages/members/member-detail/member-detail';

import { BookList } from './pages/books/book-list/book-list';
import { BookForm } from './pages/books/book-form/book-form';
import { BookDetail } from './pages/books/book-detail/book-detail';

import { LoanList } from './pages/loans/loan-list/loan-list';
import { NotFound } from './pages/not-found/not-found';


export const routes: Routes = [
  {
    path: '',
    component: Home
  },

  {
    path: 'categories',
    component: CategoryList
  },
  {
    path: 'categories/create',
    component: CategoryForm
  },
  {
    path: 'categories/edit/:id',
    component: CategoryForm
  },
  {
    path: 'categories/detail/:id',
    component: CategoryDetail
  },

  {
  path: 'authors',
  component: AuthorList
  },
  {
  path: 'authors/create',
  component: AuthorForm
  },
  {
  path: 'authors/edit/:id',
  component: AuthorForm
  },
  {
  path: 'authors/detail/:id',
  component: AuthorDetail
  },

  {
  path: 'members',
  component: MemberList
  },
  {
  path: 'members/create',
  component: MemberForm
  },
  {
  path: 'members/edit/:id',
  component: MemberForm
  },
  {
  path: 'members/detail/:id',
  component: MemberDetail
  },

 {
  path: 'books',
  component: BookList
},
{
  path: 'books/create',
  component: BookForm
},
{
  path: 'books/edit/:id',
  component: BookForm
},
{
  path: 'books/detail/:id',
  component: BookDetail
},

  {
    path: 'loans',
    component: LoanList
  },
  {
    path: '**',
    component: NotFound
  }
];