import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { LoanService } from '../../../core/services/loan.service';
import { LoanResponse } from '../../../core/models/response/loan-response';

@Component({
  selector: 'app-loan-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './loan-list.html',
  styleUrl: './loan-list.css'
})
export class LoanList implements OnInit {
  loans: LoanResponse[] = [];
  isLoading = false;
  errorMessage = '';
  processingId: number | null = null;

  constructor(private readonly loanService: LoanService) {}

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.loanService.getAll().subscribe({
      next: (loans) => {
        this.loans = loans;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar los préstamos. Verifica que la API esté ejecutándose.';
        this.isLoading = false;
      }
    });
  }

  returnLoan(loan: LoanResponse): void {
    const confirmed = window.confirm(`¿Deseas registrar la devolución del libro "${loan.bookTitle}"?`);

    if (!confirmed) {
      return;
    }

    this.processingId = loan.id;
    this.errorMessage = '';

    this.loanService.returnLoan(loan.id).subscribe({
      next: () => {
        this.processingId = null;
        this.loadLoans();
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage = error.error?.message || 'No fue posible registrar la devolución del préstamo.';
        this.processingId = null;
      }
    });
  }

  deleteLoan(loan: LoanResponse): void {
    const confirmed = window.confirm(`¿Deseas eliminar el préstamo del libro "${loan.bookTitle}"?`);

    if (!confirmed) {
      return;
    }

    this.processingId = loan.id;
    this.errorMessage = '';

    this.loanService.delete(loan.id).subscribe({
      next: () => {
        this.loans = this.loans.filter(item => item.id !== loan.id);
        this.processingId = null;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage = error.error?.message || 'No fue posible eliminar el préstamo.';
        this.processingId = null;
      }
    });
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'Active':
        return 'Activo';
      case 'Returned':
        return 'Devuelto';
      case 'Overdue':
        return 'Vencido';
      default:
        return status;
    }
  }

  getStatusBadgeClass(status: string): string {
    switch (status) {
      case 'Active':
        return 'bg-primary';
      case 'Returned':
        return 'bg-success';
      case 'Overdue':
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }

  canReturn(loan: LoanResponse): boolean {
    return loan.status === 'Active' || loan.status === 'Overdue';
  }

  canDelete(loan: LoanResponse): boolean {
    return loan.status === 'Returned';
  }
}