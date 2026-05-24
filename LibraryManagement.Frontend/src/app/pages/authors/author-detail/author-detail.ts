import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { AuthorService } from '../../../core/services/author.service';
import { AuthorResponse } from '../../../core/models/response/author-response';

@Component({
  selector: 'app-author-detail',
  imports: [CommonModule, RouterLink],
  templateUrl: './author-detail.html',
  styleUrl: './author-detail.css'
})
export class AuthorDetail implements OnInit {
  author?: AuthorResponse;
  isLoading = false;
  errorMessage = '';

  constructor(
    private readonly authorService: AuthorService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'El Id del autor no es válido.';
      return;
    }

    this.loadAuthor(id);
  }

  loadAuthor(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.authorService.getById(id).subscribe({
      next: (author) => {
        this.author = author;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar el autor solicitado.';
        this.isLoading = false;
      }
    });
  }
}