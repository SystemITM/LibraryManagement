using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;
//Este servicio  manejará la lógica real del préstamo.
public interface ILoanService
{
    Task<IEnumerable<Loan>> GetAllAsync();

    Task<Loan?> GetByIdAsync(int id);

    Task<Loan> CreateLoanAsync(int bookId, int memberId, DateTime dueDate);

    Task<Loan> ReturnLoanAsync(int loanId);

    Task<bool> DeleteAsync(int id);
}