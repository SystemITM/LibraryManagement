import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { BookService } from '../../../core/services/book.service';
import { CategoryService } from '../../../core/services/category.service';
import { AuthorService } from '../../../core/services/author.service';

import { CategoryResponse } from '../../../core/models/response/category-response';
import { AuthorResponse } from '../../../core/models/response/author-response';
import { CreateBookRequest } from '../../../core/models/request/create-book-request';
import { UpdateBookRequest } from '../../../core/models/request/update-book-request';

@Component({
  selector: 'app-book-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './book-form.html',
  styleUrl: './book-form.css'
})
export class BookForm implements OnInit {
  book = {
    title: '',
    isbn: '',
    publicationYear: new Date().getFullYear(),
    totalCopies: 1,
    availableCopies: 1,
    categoryId: 0
  };

  selectedAuthorIds: number[] = [];

  categories: CategoryResponse[] = [];
  authors: AuthorResponse[] = [];

  bookId: number | null = null;
  isEditMode = false;
  isLoading = false;
  isSaving = false;
  errorMessage = '';

  constructor(
    private readonly bookService: BookService,
    private readonly categoryService: CategoryService,
    private readonly authorService: AuthorService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.loadCatalogs();

    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.bookId = Number(idParam);
      this.isEditMode = true;
      this.loadBook(this.bookId);
    }
  }

  loadCatalogs(): void {
    this.categoryService.getAll().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar las categorías.';
      }
    });

    this.authorService.getAll().subscribe({
      next: (authors) => {
        this.authors = authors;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar los autores.';
      }
    });
  }

  loadBook(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.bookService.getById(id).subscribe({
      next: (book) => {
        this.book = {
          title: book.title,
          isbn: book.isbn,
          publicationYear: book.publicationYear,
          totalCopies: book.totalCopies,
          availableCopies: book.availableCopies,
          categoryId: book.categoryId
        };

        this.selectedAuthorIds = book.authors.map(author => author.authorId);

        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar el libro seleccionado.';
        this.isLoading = false;
      }
    });
  }

  saveBook(): void {
    this.errorMessage = '';

    if (!this.book.title.trim()) {
      this.errorMessage = 'El título del libro es obligatorio.';
      return;
    }

    if (!this.book.isbn.trim()) {
      this.errorMessage = 'El ISBN del libro es obligatorio.';
      return;
    }

    if (!this.book.publicationYear || this.book.publicationYear < 1000) {
      this.errorMessage = 'El año de publicación no es válido.';
      return;
    }

    if (!this.book.totalCopies || this.book.totalCopies <= 0) {
      this.errorMessage = 'El total de copias debe ser mayor a cero.';
      return;
    }

    if (!this.book.categoryId || this.book.categoryId <= 0) {
      this.errorMessage = 'Debe seleccionar una categoría.';
      return;
    }

    if (!this.selectedAuthorIds || this.selectedAuthorIds.length === 0) {
      this.errorMessage = 'Debe seleccionar al menos un autor.';
      return;
    }

    this.isSaving = true;

    if (this.isEditMode && this.bookId) {
      const updateRequest: UpdateBookRequest = {
        title: this.book.title.trim(),
        isbn: this.book.isbn.trim(),
        publicationYear: Number(this.book.publicationYear),
        totalCopies: Number(this.book.totalCopies),
        availableCopies: Number(this.book.availableCopies),
        categoryId: Number(this.book.categoryId),
        authorIds: this.selectedAuthorIds.map(id => Number(id))
      };

      this.updateBook(this.bookId, updateRequest);
      return;
    }

    const createRequest: CreateBookRequest = {
      title: this.book.title.trim(),
      isbn: this.book.isbn.trim(),
      publicationYear: Number(this.book.publicationYear),
      totalCopies: Number(this.book.totalCopies),
      categoryId: Number(this.book.categoryId),
      authorIds: this.selectedAuthorIds.map(id => Number(id))
    };

    this.createBook(createRequest);
  }

  private createBook(request: CreateBookRequest): void {
    this.bookService.create(request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/books']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible crear el libro.';
      }
    });
  }

  private updateBook(id: number, request: UpdateBookRequest): void {
    this.bookService.update(id, request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/books']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible actualizar el libro.';
      }
    });
  }
}