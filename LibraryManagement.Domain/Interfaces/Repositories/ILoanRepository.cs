using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;

public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<IEnumerable<Loan>> GetLoansWithDetailsAsync();

    Task<Loan?> GetLoanWithDetailsByIdAsync(int id);

    Task<Loan?> GetLoanForUpdateAsync(int id);

    Task<IEnumerable<Loan>> GetActiveLoansByMemberIdAsync(int memberId);

    Task<bool> HasActiveLoanForBookAsync(int bookId);

    Task<bool> HasActiveLoanForMemberAndBookAsync(int memberId, int bookId);
}