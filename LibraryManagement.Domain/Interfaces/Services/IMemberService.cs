using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;
//Este servicio manejará los miembros de la biblioteca.
public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllAsync();

    Task<Member?> GetByIdAsync(int id);

    Task<Member> CreateAsync(Member member);

    Task<Member> UpdateAsync(int id, Member member);

    Task<bool> DeleteAsync(int id);
}