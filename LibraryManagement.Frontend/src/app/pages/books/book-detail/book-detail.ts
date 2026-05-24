import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { BookService } from '../../../core/services/book.service';
import { BookResponse } from '../../../core/models/response/book-response';

@Component({
  selector: 'app-book-detail',
  imports: [CommonModule, RouterLink],
  templateUrl: './book-detail.html',
  styleUrl: './book-detail.css'
})
export class BookDetail implements OnInit {
  book?: BookResponse;
  isLoading = false;
  errorMessage = '';

  constructor(
    private readonly bookService: BookService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'El Id del libro no es válido.';
      return;
    }

    this.loadBook(id);
  }

  loadBook(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.bookService.getById(id).subscribe({
      next: (book) => {
        this.book = book;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar el libro solicitado.';
        this.isLoading = false;
      }
    });
  }

  getAuthorsText(): string {
    if (!this.book?.authors || this.book.authors.length === 0) {
      return 'Sin autores asociados';
    }

    return this.book.authors.map(author => author.fullName).join(', ');
  }
}