import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { LoanService } from '../../../core/services/loan.service';
import { BookService } from '../../../core/services/book.service';
import { MemberService } from '../../../core/services/member.service';

import { BookResponse } from '../../../core/models/response/book-response';
import { MemberResponse } from '../../../core/models/response/member-response';
import { CreateLoanRequest } from '../../../core/models/request/create-loan-request';

@Component({
  selector: 'app-loan-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './loan-form.html',
  styleUrl: './loan-form.css'
})
export class LoanForm implements OnInit {
  loan = {
    bookId: 0,
    memberId: 0,
    dueDate: ''
  };

  books: BookResponse[] = [];
  members: MemberResponse[] = [];

  isLoading = false;
  isSaving = false;
  errorMessage = '';

  constructor(
    private readonly loanService: LoanService,
    private readonly bookService: BookService,
    private readonly memberService: MemberService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.setDefaultDueDate();
    this.loadCatalogs();
  }

  setDefaultDueDate(): void {
    const date = new Date();
    date.setDate(date.getDate() + 7);

    this.loan.dueDate = date.toISOString().split('T')[0];
  }

  loadCatalogs(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.bookService.getAll().subscribe({
      next: (books) => {
        this.books = books.filter(book => book.availableCopies > 0);
        this.checkCatalogLoadingCompleted();
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar los libros.';
        this.isLoading = false;
      }
    });

    this.memberService.getAll().subscribe({
      next: (members) => {
        this.members = members.filter(member => member.isActive);
        this.checkCatalogLoadingCompleted();
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar los miembros.';
        this.isLoading = false;
      }
    });
  }

  checkCatalogLoadingCompleted(): void {
    if (this.books.length >= 0 && this.members.length >= 0) {
      this.isLoading = false;
    }
  }

  saveLoan(): void {
    this.errorMessage = '';

    if (!this.loan.bookId || this.loan.bookId <= 0) {
      this.errorMessage = 'Debe seleccionar un libro.';
      return;
    }

    if (!this.loan.memberId || this.loan.memberId <= 0) {
      this.errorMessage = 'Debe seleccionar un miembro.';
      return;
    }

    if (!this.loan.dueDate) {
      this.errorMessage = 'Debe seleccionar la fecha límite de devolución.';
      return;
    }

    const selectedDueDate = new Date(this.loan.dueDate);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (selectedDueDate <= today) {
      this.errorMessage = 'La fecha límite debe ser mayor a la fecha actual.';
      return;
    }

    this.isSaving = true;

    const request: CreateLoanRequest = {
      bookId: Number(this.loan.bookId),
      memberId: Number(this.loan.memberId),
      dueDate: `${this.loan.dueDate}T00:00:00`
    };

    this.loanService.create(request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/loans']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible crear el préstamo.';
      }
    });
  }

  getSelectedBookInfo(): string {
    const book = this.books.find(item => item.id === Number(this.loan.bookId));

    if (!book) {
      return '';
    }

    return `Copias disponibles: ${book.availableCopies}`;
  }
}