using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;
/*Para validaciones de prestamos
 Consultar préstamos con libro y miembro
Validar préstamos activos de un miembro
Validar si un libro ya tiene préstamo activo
Validar si un miembro ya tiene prestado el mismo libro
 */
public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<IEnumerable<Loan>> GetLoansWithDetailsAsync();

    Task<Loan?> GetLoanWithDetailsByIdAsync(int id);

    Task<IEnumerable<Loan>> GetActiveLoansByMemberIdAsync(int memberId);

    Task<bool> HasActiveLoanForBookAsync(int bookId);

    Task<bool> HasActiveLoanForMemberAndBookAsync(int memberId, int bookId);
}