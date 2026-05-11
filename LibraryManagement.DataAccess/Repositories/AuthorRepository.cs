using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories;

public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await GetAuthorsWithBooksAsync();
    }

    public override async Task<Author?> GetByIdAsync(int id)
    {
        return await GetAuthorWithBooksByIdAsync(id);
    }

    public async Task<IEnumerable<Author>> GetAuthorsWithBooksAsync()
    {
        return await _context.Authors
            .Include(author => author.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Book)
                    .ThenInclude(book => book.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Author?> GetAuthorWithBooksByIdAsync(int id)
    {
        return await _context.Authors
            .Include(author => author.BookAuthors)
                .ThenInclude(bookAuthor => bookAuthor.Book)
                    .ThenInclude(book => book.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(author => author.Id == id);
    }

    public async Task<bool> ExistsByFullNameAsync(string firstName, string lastName, int? excludeAuthorId = null)
    {
        var normalizedFirstName = firstName.Trim();
        var normalizedLastName = lastName.Trim();

        return await _context.Authors
            .AnyAsync(author =>
                author.FirstName == normalizedFirstName &&
                author.LastName == normalizedLastName &&
                (!excludeAuthorId.HasValue || author.Id != excludeAuthorId.Value));
    }
}