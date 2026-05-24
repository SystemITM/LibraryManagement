export interface UpdateBookRequest {
  title: string;
  isbn: string;
  publicationYear: number;
  totalCopies: number;
  availableCopies: number;
  categoryId: number;
  authorIds: number[];
}