import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { BookService } from '../../../core/services/book.service';
import { BookResponse } from '../../../core/models/response/book-response';

@Component({
  selector: 'app-book-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './book-list.html',
  styleUrl: './book-list.css'
})
export class BookList implements OnInit {
  books: BookResponse[] = [];
  isLoading = false;
  errorMessage = '';
  deletingId: number | null = null;

  constructor(private readonly bookService: BookService) {}

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.bookService.getAll().subscribe({
      next: (books) => {
        this.books = books;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar los libros. Verifica que la API esté ejecutándose.';
        this.isLoading = false;
      }
    });
  }

  getAuthorsText(book: BookResponse): string {
    if (!book.authors || book.authors.length === 0) {
      return 'Sin autores';
    }

    return book.authors.map(author => author.fullName).join(', ');
  }

  deleteBook(book: BookResponse): void {
    const confirmed = window.confirm(`¿Deseas eliminar el libro "${book.title}"?`);

    if (!confirmed) {
      return;
    }

    this.deletingId = book.id;
    this.errorMessage = '';

    this.bookService.delete(book.id).subscribe({
      next: () => {
        this.books = this.books.filter(item => item.id !== book.id);
        this.deletingId = null;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage = error.error?.message || 'No fue posible eliminar el libro.';
        this.deletingId = null;
      }
    });
  }
}