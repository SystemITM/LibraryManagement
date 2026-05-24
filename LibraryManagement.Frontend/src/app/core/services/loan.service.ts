import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_BASE_URL } from './api.config';
import { LoanResponse } from '../models/response/loan-response';
import { CreateLoanRequest } from '../models/request/create-loan-request';

@Injectable({
  providedIn: 'root'
})
export class LoanService {
  private readonly apiUrl = `${API_BASE_URL}/loans`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<LoanResponse[]> {
    return this.http.get<LoanResponse[]>(this.apiUrl);
  }

  getById(id: number): Observable<LoanResponse> {
    return this.http.get<LoanResponse>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateLoanRequest): Observable<LoanResponse> {
    return this.http.post<LoanResponse>(this.apiUrl, request);
  }

  returnLoan(id: number): Observable<LoanResponse> {
    return this.http.put<LoanResponse>(`${this.apiUrl}/${id}/return`, {});
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}