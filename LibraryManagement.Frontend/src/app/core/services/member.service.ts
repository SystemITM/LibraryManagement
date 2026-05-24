import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_BASE_URL } from './api.config';
import { MemberResponse } from '../models/response/member-response';
import { CreateMemberRequest } from '../models/request/create-member-request';
import { UpdateMemberRequest } from '../models/request/update-member-request';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private readonly apiUrl = `${API_BASE_URL}/members`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<MemberResponse[]> {
    return this.http.get<MemberResponse[]>(this.apiUrl);
  }

  getById(id: number): Observable<MemberResponse> {
    return this.http.get<MemberResponse>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateMemberRequest): Observable<MemberResponse> {
    return this.http.post<MemberResponse>(this.apiUrl, request);
  }

  update(id: number, request: UpdateMemberRequest): Observable<MemberResponse> {
    return this.http.put<MemberResponse>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}