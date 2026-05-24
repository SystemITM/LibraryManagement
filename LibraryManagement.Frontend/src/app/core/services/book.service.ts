import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_BASE_URL } from './api.config';
import { BookResponse } from '../models/response/book-response';
import { CreateBookRequest } from '../models/request/create-book-request';
import { UpdateBookRequest } from '../models/request/update-book-request';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private readonly apiUrl = `${API_BASE_URL}/books`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<BookResponse[]> {
    return this.http.get<BookResponse[]>(this.apiUrl);
  }

  getById(id: number): Observable<BookResponse> {
    return this.http.get<BookResponse>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateBookRequest): Observable<BookResponse> {
    return this.http.post<BookResponse>(this.apiUrl, request);
  }

  update(id: number, request: UpdateBookRequest): Observable<BookResponse> {
    return this.http.put<BookResponse>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}