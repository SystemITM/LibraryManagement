import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_BASE_URL } from './api.config';
import { AuthorResponse } from '../models/response/author-response';
import { CreateAuthorRequest } from '../models/request/create-author-request';
import { UpdateAuthorRequest } from '../models/request/update-author-request';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private readonly apiUrl = `${API_BASE_URL}/authors`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<AuthorResponse[]> {
    return this.http.get<AuthorResponse[]>(this.apiUrl);
  }

  getById(id: number): Observable<AuthorResponse> {
    return this.http.get<AuthorResponse>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateAuthorRequest): Observable<AuthorResponse> {
    return this.http.post<AuthorResponse>(this.apiUrl, request);
  }

  update(id: number, request: UpdateAuthorRequest): Observable<AuthorResponse> {
    return this.http.put<AuthorResponse>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}