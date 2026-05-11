using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await GetBooksWithDetailsAsync();
    }

    public override async Task<Book?> GetByIdAsync(int id)
    {
        return await GetBookWithDetailsByIdAsync(id);
    }

    public async Task<IEnumerable<Book>> GetBooksWithDetailsAsync()
    {
        return await _context.Books
            .Include(book => book.Category)
            .Include(book => book.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Author)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> GetBookWithDetailsByIdAsync(int id)
    {
        return await _context.Books
            .Include(book => book.Category)
            .Include(book => book.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Author)
            .Include(book => book.Loans)
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == id);
    }

    public async Task<Book?> GetBookForUpdateAsync(int id)
    {
        return await _context.Books
            .FirstOrDefaultAsync(book => book.Id == id);
    }

    public async Task<bool> ExistsByIsbnAsync(string isbn, int? excludeBookId = null)
    {
        var normalizedIsbn = isbn.Trim();

        return await _context.Books
            .AnyAsync(book =>
                book.Isbn == normalizedIsbn &&
                (!excludeBookId.HasValue || book.Id != excludeBookId.Value));
    }

    public async Task UpdateBookAuthorsAsync(int bookId, IEnumerable<int> authorIds)
    {
        var currentRelations = await _context.BookAuthors
            .Where(bookAuthor => bookAuthor.BookId == bookId)
            .ToListAsync();

        _context.BookAuthors.RemoveRange(currentRelations);

        var newRelations = authorIds
            .Distinct()
            .Select(authorId => new BookAuthor
            {
                BookId = bookId,
                AuthorId = authorId
            })
            .ToList();

        await _context.BookAuthors.AddRangeAsync(newRelations);
        await _context.SaveChangesAsync();
    }
}