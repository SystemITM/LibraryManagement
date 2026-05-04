using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;

//permitirá consultar miembros con sus préstamos y validar duplicados por: correo electronico
public interface IMemberRepository : IGenericRepository<Member>
{
    Task<IEnumerable<Member>> GetMembersWithLoansAsync();

    Task<Member?> GetMemberWithLoansByIdAsync(int id);

    Task<bool> ExistsByDocumentNumberAsync(string documentNumber, int? excludeMemberId = null);

    Task<bool> ExistsByEmailAsync(string email, int? excludeMemberId = null);
}