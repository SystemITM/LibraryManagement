export interface CategoryResponse {
  id: number;
  name: string;
  description?: string;
  bookCount: number;
  createdAt: string;
  updatedAt?: string;
}