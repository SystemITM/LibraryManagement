import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { CategoryService } from '../../../core/services/category.service';
import { CreateCategoryRequest } from '../../../core/models/request/create-category-request';

@Component({
  selector: 'app-category-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './category-form.html',
  styleUrl: './category-form.css'
})
export class CategoryForm {
  category: CreateCategoryRequest = {
    name: '',
    description: ''
  };

  isSaving = false;
  errorMessage = '';

  constructor(
    private readonly categoryService: CategoryService,
    private readonly router: Router
  ) {}

  saveCategory(): void {
    this.errorMessage = '';

    if (!this.category.name.trim()) {
      this.errorMessage = 'El nombre de la categoría es obligatorio.';
      return;
    }

    this.isSaving = true;

    const request: CreateCategoryRequest = {
      name: this.category.name.trim(),
      description: this.category.description?.trim() || undefined
    };

    this.categoryService.create(request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/categories']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible crear la categoría.';
      }
    });
  }
}