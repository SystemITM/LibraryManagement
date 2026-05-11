using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;

    public LoanService(
        ILoanRepository loanRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _loanRepository.GetAllAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _loanRepository.GetByIdAsync(id);
    }

    public async Task<Loan> CreateLoanAsync(int bookId, int memberId, DateTime dueDate)
    {
        if (bookId <= 0)
        {
            throw new InvalidOperationException("El Id del libro no es válido.");
        }

        if (memberId <= 0)
        {
            throw new InvalidOperationException("El Id del miembro no es válido.");
        }

        if (dueDate.Date <= DateTime.Now.Date)
        {
            throw new InvalidOperationException("La fecha esperada de devolución debe ser mayor a la fecha actual.");
        }

        var book = await _bookRepository.GetBookForUpdateAsync(bookId);

        if (book is null)
        {
            throw new KeyNotFoundException("El libro no existe.");
        }

        var member = await _memberRepository.GetByIdAsync(memberId);

        if (member is null)
        {
            throw new KeyNotFoundException("El miembro no existe.");
        }

        if (!member.IsActive)
        {
            throw new InvalidOperationException("El miembro no está activo y no puede realizar préstamos.");
        }

        if (book.AvailableCopies <= 0)
        {
            throw new InvalidOperationException("El libro no tiene copias disponibles para préstamo.");
        }

        if (await _loanRepository.HasActiveLoanForMemberAndBookAsync(memberId, bookId))
        {
            throw new InvalidOperationException("El miembro ya tiene un préstamo activo para este libro.");
        }

        book.AvailableCopies--;

        await _bookRepository.UpdateAsync(book);

        var loan = new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            LoanDate = DateTime.Now,
            DueDate = dueDate,
            ReturnDate = null,
            Status = LoanStatus.Active
        };

        return await _loanRepository.CreateAsync(loan);
    }

    public async Task<Loan> ReturnLoanAsync(int loanId)
    {
        var loan = await _loanRepository.GetLoanForUpdateAsync(loanId);

        if (loan is null)
        {
            throw new KeyNotFoundException("El préstamo no existe.");
        }

        if (loan.Status == LoanStatus.Returned)
        {
            throw new InvalidOperationException("El préstamo ya fue devuelto.");
        }

        var book = await _bookRepository.GetBookForUpdateAsync(loan.BookId);

        if (book is null)
        {
            throw new KeyNotFoundException("El libro asociado al préstamo no existe.");
        }

        loan.ReturnDate = DateTime.Now;
        loan.Status = LoanStatus.Returned;

        book.AvailableCopies++;

        if (book.AvailableCopies > book.TotalCopies)
        {
            book.AvailableCopies = book.TotalCopies;
        }

        await _loanRepository.UpdateAsync(loan);
        await _bookRepository.UpdateAsync(book);

        var updatedLoan = await _loanRepository.GetByIdAsync(loan.Id);

        return updatedLoan!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var loan = await _loanRepository.GetByIdAsync(id);

        if (loan is null)
        {
            return false;
        }

        if (loan.Status == LoanStatus.Active)
        {
            throw new InvalidOperationException("No se puede eliminar un préstamo activo.");
        }

        return await _loanRepository.DeleteAsync(id);
    }
}