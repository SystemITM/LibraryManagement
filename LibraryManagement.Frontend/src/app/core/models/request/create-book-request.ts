export interface CreateBookRequest {
  title: string;
  isbn: string;
  publicationYear: number;
  totalCopies: number;
  categoryId: number;
  authorIds: number[];
}