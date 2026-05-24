import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { CategoryList } from './pages/categories/category-list/category-list';
import { CategoryForm } from './pages/categories/category-form/category-form';
import { CategoryDetail } from './pages/categories/category-detail/category-detail';
import { AuthorList } from './pages/authors/author-list/author-list';
import { MemberList } from './pages/members/member-list/member-list';
import { BookList } from './pages/books/book-list/book-list';
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
    path: 'members',
    component: MemberList
  },
  {
    path: 'books',
    component: BookList
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