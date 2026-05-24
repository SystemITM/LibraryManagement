import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { MemberService } from '../../../core/services/member.service';
import { MemberResponse } from '../../../core/models/response/member-response';

@Component({
  selector: 'app-member-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './member-list.html',
  styleUrl: './member-list.css'
})
export class MemberList implements OnInit {
  members: MemberResponse[] = [];
  isLoading = false;
  errorMessage = '';
  deletingId: number | null = null;

  constructor(private readonly memberService: MemberService) {}

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.memberService.getAll().subscribe({
      next: (members) => {
        this.members = members;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar los miembros. Verifica que la API esté ejecutándose.';
        this.isLoading = false;
      }
    });
  }

  deleteMember(member: MemberResponse): void {
    const confirmed = window.confirm(`¿Deseas eliminar el miembro "${member.fullName}"?`);

    if (!confirmed) {
      return;
    }

    this.deletingId = member.id;
    this.errorMessage = '';

    this.memberService.delete(member.id).subscribe({
      next: () => {
        this.members = this.members.filter(item => item.id !== member.id);
        this.deletingId = null;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage = error.error?.message || 'No fue posible eliminar el miembro.';
        this.deletingId = null;
      }
    });
  }
}