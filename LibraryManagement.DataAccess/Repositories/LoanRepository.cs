using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories;

public class LoanRepository : GenericRepository<Loan>, ILoanRepository
{
    public LoanRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await GetLoansWithDetailsAsync();
    }

    public override async Task<Loan?> GetByIdAsync(int id)
    {
        return await GetLoanWithDetailsByIdAsync(id);
    }

    public async Task<IEnumerable<Loan>> GetLoansWithDetailsAsync()
    {
        return await _context.Loans
            .Include(loan => loan.Book)
                .ThenInclude(book => book.Category)
            .Include(loan => loan.Book)
                .ThenInclude(book => book.BookAuthors)
                    .ThenInclude(bookAuthor => bookAuthor.Author)
            .Include(loan => loan.Member)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Loan?> GetLoanWithDetailsByIdAsync(int id)
    {
        return await _context.Loans
            .Include(loan => loan.Book)
                .ThenInclude(book => book.Category)
            .Include(loan => loan.Book)
                .ThenInclude(book => book.BookAuthors)
                    .ThenInclude(bookAuthor => bookAuthor.Author)
            .Include(loan => loan.Member)
            .AsNoTracking()
            .FirstOrDefaultAsync(loan => loan.Id == id);
    }

    public async Task<Loan?> GetLoanForUpdateAsync(int id)
    {
        return await _context.Loans
            .FirstOrDefaultAsync(loan => loan.Id == id);
    }

    public async Task<IEnumerable<Loan>> GetActiveLoansByMemberIdAsync(int memberId)
    {
        return await _context.Loans
            .Include(loan => loan.Book)
            .Include(loan => loan.Member)
            .Where(loan =>
                loan.MemberId == memberId &&
                loan.Status == LoanStatus.Active)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> HasActiveLoanForBookAsync(int bookId)
    {
        return await _context.Loans
            .AnyAsync(loan =>
                loan.BookId == bookId &&
                loan.Status == LoanStatus.Active);
    }

    public async Task<bool> HasActiveLoanForMemberAndBookAsync(int memberId, int bookId)
    {
        return await _context.Loans
            .AnyAsync(loan =>
                loan.MemberId == memberId &&
                loan.BookId == bookId &&
                loan.Status == LoanStatus.Active);
    }
}