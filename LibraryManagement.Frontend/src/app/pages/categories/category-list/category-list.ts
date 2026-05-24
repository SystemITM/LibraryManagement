import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { CategoryService } from '../../../core/services/category.service';
import { CategoryResponse } from '../../../core/models/response/category-response';

@Component({
  selector: 'app-category-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './category-list.html',
  styleUrl: './category-list.css'
})
export class CategoryList implements OnInit {
  categories: CategoryResponse[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(private readonly categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.categoryService.getAll().subscribe({
      next: (categories) => {
        this.categories = categories;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar las categorías. Verifica que la API esté ejecutándose.';
        this.isLoading = false;
      }
    });
  }
}