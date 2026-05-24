import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { CategoryService } from '../../../core/services/category.service';
import { CategoryResponse } from '../../../core/models/response/category-response';

@Component({
  selector: 'app-category-detail',
  imports: [CommonModule, RouterLink],
  templateUrl: './category-detail.html',
  styleUrl: './category-detail.css'
})
export class CategoryDetail implements OnInit {
  category?: CategoryResponse;
  isLoading = false;
  errorMessage = '';

  constructor(
    private readonly categoryService: CategoryService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'El Id de la categoría no es válido.';
      return;
    }

    this.loadCategory(id);
  }

  loadCategory(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.categoryService.getById(id).subscribe({
      next: (category) => {
        this.category = category;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar la categoría solicitada.';
        this.isLoading = false;
      }
    });
  }
}