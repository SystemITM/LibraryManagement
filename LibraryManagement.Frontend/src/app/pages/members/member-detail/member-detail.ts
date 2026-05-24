import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { MemberService } from '../../../core/services/member.service';
import { MemberResponse } from '../../../core/models/response/member-response';

@Component({
  selector: 'app-member-detail',
  imports: [CommonModule, RouterLink],
  templateUrl: './member-detail.html',
  styleUrl: './member-detail.css'
})
export class MemberDetail implements OnInit {
  member?: MemberResponse;
  isLoading = false;
  errorMessage = '';

  constructor(
    private readonly memberService: MemberService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'El Id del miembro no es válido.';
      return;
    }

    this.loadMember(id);
  }

  loadMember(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.memberService.getById(id).subscribe({
      next: (member) => {
        this.member = member;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar el miembro solicitado.';
        this.isLoading = false;
      }
    });
  }
}