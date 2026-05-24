import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { AuthorService } from '../../../core/services/author.service';
import { CreateAuthorRequest } from '../../../core/models/request/create-author-request';
import { UpdateAuthorRequest } from '../../../core/models/request/update-author-request';

@Component({
  selector: 'app-author-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './author-form.html',
  styleUrl: './author-form.css'
})
export class AuthorForm implements OnInit {
  author: CreateAuthorRequest | UpdateAuthorRequest = {
    firstName: '',
    lastName: '',
    biography: ''
  };

  authorId: number | null = null;
  isEditMode = false;
  isLoading = false;
  isSaving = false;
  errorMessage = '';

  constructor(
    private readonly authorService: AuthorService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.authorId = Number(idParam);
      this.isEditMode = true;
      this.loadAuthor(this.authorId);
    }
  }

  loadAuthor(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.authorService.getById(id).subscribe({
      next: (author) => {
        this.author = {
          firstName: author.firstName,
          lastName: author.lastName,
          biography: author.biography
        };

        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar el autor seleccionado.';
        this.isLoading = false;
      }
    });
  }

  saveAuthor(): void {
    this.errorMessage = '';

    if (!this.author.firstName.trim()) {
      this.errorMessage = 'El nombre del autor es obligatorio.';
      return;
    }

    if (!this.author.lastName.trim()) {
      this.errorMessage = 'El apellido del autor es obligatorio.';
      return;
    }

    this.isSaving = true;

    const request = {
      firstName: this.author.firstName.trim(),
      lastName: this.author.lastName.trim(),
      biography: this.author.biography?.trim() || undefined
    };

    if (this.isEditMode && this.authorId) {
      this.updateAuthor(this.authorId, request);
      return;
    }

    this.createAuthor(request);
  }

  private createAuthor(request: CreateAuthorRequest): void {
    this.authorService.create(request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/authors']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible crear el autor.';
      }
    });
  }

  private updateAuthor(id: number, request: UpdateAuthorRequest): void {
    this.authorService.update(id, request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/authors']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible actualizar el autor.';
      }
    });
  }
}