using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _memberRepository.GetAllAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _memberRepository.GetByIdAsync(id);
    }

    public async Task<Member> CreateAsync(Member member)
    {
        ValidateMember(member);

        member.FirstName = member.FirstName.Trim();
        member.LastName = member.LastName.Trim();
        member.DocumentNumber = member.DocumentNumber.Trim();
        member.Email = member.Email.Trim();
        member.PhoneNumber = member.PhoneNumber.Trim();

        if (await _memberRepository.ExistsByDocumentNumberAsync(member.DocumentNumber))
        {
            throw new InvalidOperationException("Ya existe un miembro con el mismo número de documento.");
        }

        if (await _memberRepository.ExistsByEmailAsync(member.Email))
        {
            throw new InvalidOperationException("Ya existe un miembro con el mismo correo electrónico.");
        }

        return await _memberRepository.CreateAsync(member);
    }

    public async Task<Member> UpdateAsync(int id, Member member)
    {
        ValidateMember(member);

        var existingMember = await _memberRepository.GetByIdAsync(id);

        if (existingMember is null)
        {
            throw new KeyNotFoundException("El miembro no existe.");
        }

        if (await _memberRepository.ExistsByDocumentNumberAsync(member.DocumentNumber, id))
        {
            throw new InvalidOperationException("Ya existe otro miembro con el mismo número de documento.");
        }

        if (await _memberRepository.ExistsByEmailAsync(member.Email, id))
        {
            throw new InvalidOperationException("Ya existe otro miembro con el mismo correo electrónico.");
        }

        existingMember.FirstName = member.FirstName.Trim();
        existingMember.LastName = member.LastName.Trim();
        existingMember.DocumentNumber = member.DocumentNumber.Trim();
        existingMember.Email = member.Email.Trim();
        existingMember.PhoneNumber = member.PhoneNumber.Trim();
        existingMember.IsActive = member.IsActive;

        return await _memberRepository.UpdateAsync(existingMember);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var member = await _memberRepository.GetMemberWithLoansByIdAsync(id);

        if (member is null)
        {
            return false;
        }

        if (member.Loans.Any())
        {
            throw new InvalidOperationException("No se puede eliminar un miembro que tiene préstamos registrados.");
        }

        return await _memberRepository.DeleteAsync(id);
    }

    private static void ValidateMember(Member member)
    {
        if (string.IsNullOrWhiteSpace(member.FirstName))
        {
            throw new InvalidOperationException("El nombre del miembro es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(member.LastName))
        {
            throw new InvalidOperationException("El apellido del miembro es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(member.DocumentNumber))
        {
            throw new InvalidOperationException("El número de documento es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(member.Email))
        {
            throw new InvalidOperationException("El correo electrónico es obligatorio.");
        }

        if (!member.Email.Contains('@'))
        {
            throw new InvalidOperationException("El correo electrónico no tiene un formato válido.");
        }

        if (string.IsNullOrWhiteSpace(member.PhoneNumber))
        {
            throw new InvalidOperationException("El teléfono es obligatorio.");
        }
    }
}