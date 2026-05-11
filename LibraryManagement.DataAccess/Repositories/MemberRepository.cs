using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{
    public MemberRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await GetMembersWithLoansAsync();
    }

    public override async Task<Member?> GetByIdAsync(int id)
    {
        return await GetMemberWithLoansByIdAsync(id);
    }

    public async Task<IEnumerable<Member>> GetMembersWithLoansAsync()
    {
        return await _context.Members
            .Include(member => member.Loans)
                .ThenInclude(loan => loan.Book)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Member?> GetMemberWithLoansByIdAsync(int id)
    {
        return await _context.Members
            .Include(member => member.Loans)
                .ThenInclude(loan => loan.Book)
            .AsNoTracking()
            .FirstOrDefaultAsync(member => member.Id == id);
    }

    public async Task<bool> ExistsByDocumentNumberAsync(string documentNumber, int? excludeMemberId = null)
    {
        var normalizedDocumentNumber = documentNumber.Trim();

        return await _context.Members
            .AnyAsync(member =>
                member.DocumentNumber == normalizedDocumentNumber &&
                (!excludeMemberId.HasValue || member.Id != excludeMemberId.Value));
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeMemberId = null)
    {
        var normalizedEmail = email.Trim();

        return await _context.Members
            .AnyAsync(member =>
                member.Email == normalizedEmail &&
                (!excludeMemberId.HasValue || member.Id != excludeMemberId.Value));
    }
}