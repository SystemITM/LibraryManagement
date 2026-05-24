export interface LoanResponse {
  id: number;
  bookId: number;
  bookTitle: string;
  memberId: number;
  memberFullName: string;
  loanDate: string;
  dueDate: string;
  returnDate?: string;
  status: string;
  createdAt: string;
  updatedAt?: string;
}