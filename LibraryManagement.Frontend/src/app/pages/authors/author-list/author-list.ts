import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthorService } from '../../../core/services/author.service';
import { AuthorResponse } from '../../../core/models/response/author-response';

@Component({
  selector: 'app-author-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './author-list.html',
  styleUrl: './author-list.css'
})
export class AuthorList implements OnInit {
  authors: AuthorResponse[] = [];
  isLoading = false;
  errorMessage = '';
  deletingId: number | null = null;

  constructor(private readonly authorService: AuthorService) {}

  ngOnInit(): void {
    this.loadAuthors();
  }

  loadAuthors(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.authorService.getAll().subscribe({
      next: (authors) => {
        this.authors = authors;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar los autores. Verifica que la API esté ejecutándose.';
        this.isLoading = false;
      }
    });
  }

  deleteAuthor(author: AuthorResponse): void {
    const confirmed = window.confirm(`¿Deseas eliminar el autor "${author.fullName}"?`);

    if (!confirmed) {
      return;
    }

    this.deletingId = author.id;
    this.errorMessage = '';

    this.authorService.delete(author.id).subscribe({
      next: () => {
        this.authors = this.authors.filter(item => item.id !== author.id);
        this.deletingId = null;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage = error.error?.message || 'No fue posible eliminar el autor.';
        this.deletingId = null;
      }
    });
  }
}