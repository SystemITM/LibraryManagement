export interface MemberResponse {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  documentNumber: string;
  email: string;
  phoneNumber: string;
  isActive: boolean;
  loanCount: number;
  createdAt: string;
  updatedAt?: string;
}