import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { CategoryService } from '../../../core/services/category.service';
import { CreateCategoryRequest } from '../../../core/models/request/create-category-request';
import { UpdateCategoryRequest } from '../../../core/models/request/update-category-request';

@Component({
  selector: 'app-category-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './category-form.html',
  styleUrl: './category-form.css'
})
export class CategoryForm implements OnInit {
  category: CreateCategoryRequest | UpdateCategoryRequest = {
    name: '',
    description: ''
  };

  categoryId: number | null = null;
  isEditMode = false;
  isLoading = false;
  isSaving = false;
  errorMessage = '';

  constructor(
    private readonly categoryService: CategoryService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.categoryId = Number(idParam);
      this.isEditMode = true;
      this.loadCategory(this.categoryId);
    }
  }

  loadCategory(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.categoryService.getById(id).subscribe({
      next: (category) => {
        this.category = {
          name: category.name,
          description: category.description
        };

        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar la categoría seleccionada.';
        this.isLoading = false;
      }
    });
  }

  saveCategory(): void {
    this.errorMessage = '';

    if (!this.category.name.trim()) {
      this.errorMessage = 'El nombre de la categoría es obligatorio.';
      return;
    }

    this.isSaving = true;

    const request = {
      name: this.category.name.trim(),
      description: this.category.description?.trim() || undefined
    };

    if (this.isEditMode && this.categoryId) {
      this.updateCategory(this.categoryId, request);
      return;
    }

    this.createCategory(request);
  }

  private createCategory(request: CreateCategoryRequest): void {
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

  private updateCategory(id: number, request: UpdateCategoryRequest): void {
    this.categoryService.update(id, request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/categories']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible actualizar la categoría.';
      }
    });
  }
}