import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { MemberService } from '../../../core/services/member.service';
import { CreateMemberRequest } from '../../../core/models/request/create-member-request';
import { UpdateMemberRequest } from '../../../core/models/request/update-member-request';

@Component({
  selector: 'app-member-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './member-form.html',
  styleUrl: './member-form.css'
})
export class MemberForm implements OnInit {
  member: CreateMemberRequest | UpdateMemberRequest = {
    firstName: '',
    lastName: '',
    documentNumber: '',
    email: '',
    phoneNumber: '',
    isActive: true
  } as UpdateMemberRequest;

  memberId: number | null = null;
  isEditMode = false;
  isLoading = false;
  isSaving = false;
  errorMessage = '';

  constructor(
    private readonly memberService: MemberService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.memberId = Number(idParam);
      this.isEditMode = true;
      this.loadMember(this.memberId);
    }
  }

  loadMember(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.memberService.getById(id).subscribe({
      next: (member) => {
        this.member = {
          firstName: member.firstName,
          lastName: member.lastName,
          documentNumber: member.documentNumber,
          email: member.email,
          phoneNumber: member.phoneNumber,
          isActive: member.isActive
        };

        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'No fue posible cargar el miembro seleccionado.';
        this.isLoading = false;
      }
    });
  }

  saveMember(): void {
    this.errorMessage = '';

    if (!this.member.firstName.trim()) {
      this.errorMessage = 'El nombre del miembro es obligatorio.';
      return;
    }

    if (!this.member.lastName.trim()) {
      this.errorMessage = 'El apellido del miembro es obligatorio.';
      return;
    }

    if (!this.member.documentNumber.trim()) {
      this.errorMessage = 'El número de documento es obligatorio.';
      return;
    }

    if (!this.member.email.trim()) {
      this.errorMessage = 'El correo electrónico es obligatorio.';
      return;
    }

    if (!this.member.phoneNumber.trim()) {
      this.errorMessage = 'El teléfono es obligatorio.';
      return;
    }

    this.isSaving = true;

    if (this.isEditMode && this.memberId) {
      const updateRequest: UpdateMemberRequest = {
        firstName: this.member.firstName.trim(),
        lastName: this.member.lastName.trim(),
        documentNumber: this.member.documentNumber.trim(),
        email: this.member.email.trim(),
        phoneNumber: this.member.phoneNumber.trim(),
        isActive: (this.member as UpdateMemberRequest).isActive
      };

      this.updateMember(this.memberId, updateRequest);
      return;
    }

    const createRequest: CreateMemberRequest = {
      firstName: this.member.firstName.trim(),
      lastName: this.member.lastName.trim(),
      documentNumber: this.member.documentNumber.trim(),
      email: this.member.email.trim(),
      phoneNumber: this.member.phoneNumber.trim()
    };

    this.createMember(createRequest);
  }

  private createMember(request: CreateMemberRequest): void {
    this.memberService.create(request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/members']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible crear el miembro.';
      }
    });
  }

  private updateMember(id: number, request: UpdateMemberRequest): void {
    this.memberService.update(id, request).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/members']);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = error.error?.message || 'No fue posible actualizar el miembro.';
      }
    });
  }
}