export interface AuthorResponse {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  biography?: string;
  bookCount: number;
  createdAt: string;
  updatedAt?: string;
}