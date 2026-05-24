import { BookAuthorResponse } from './book-author-response';

export interface BookResponse {
  id: number;
  title: string;
  isbn: string;
  publicationYear: number;
  totalCopies: number;
  availableCopies: number;
  categoryId: number;
  categoryName: string;
  authors: BookAuthorResponse[];
  createdAt: string;
  updatedAt?: string;
}