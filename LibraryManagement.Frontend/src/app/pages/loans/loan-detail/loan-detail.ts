import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { LoanService } from '../../../core/services/loan.service';
import { LoanResponse } from '../../../core/models/response/loan-response';

@Component({
  selector: 'app-loan-detail',
  imports: [CommonModule, RouterLink],
  templateUrl: './loan-detail.html',
  styleUrl: './loan-detail.css'
})
export class LoanDetail implements OnInit {
  loan?: LoanResponse;
  isLoading = false;
  isProcessing = false;
  errorMessage = '';

  constructor(
    private readonly loanService: LoanService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'El Id del préstamo no es válido.';
      return;
    }

    this.loadLoan(id);
  }

  loadLoan(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.loanService.getById(id).subscribe({
      next: (loan) => {
        this.loan = loan;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar el préstamo solicitado.';
        this.isLoading = false;
      }
    });
  }

  returnLoan(): void {
    if (!this.loan) {
      return;
    }

    const confirmed = window.confirm(`¿Deseas registrar la devolución del libro "${this.loan.bookTitle}"?`);

    if (!confirmed) {
      return;
    }

    this.isProcessing = true;
    this.errorMessage = '';

    this.loanService.returnLoan(this.loan.id).subscribe({
      next: (returnedLoan) => {
        this.loan = returnedLoan;
        this.isProcessing = false;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage = error.error?.message || 'No fue posible registrar la devolución del préstamo.';
        this.isProcessing = false;
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

  canReturn(): boolean {
    return this.loan?.status === 'Active' || this.loan?.status === 'Overdue';
  }
}