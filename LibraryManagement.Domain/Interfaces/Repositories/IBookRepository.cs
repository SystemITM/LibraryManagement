using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksWithDetailsAsync();

    Task<Book?> GetBookWithDetailsByIdAsync(int id);

    Task<bool> ExistsByIsbnAsync(string isbn, int? excludeBookId = null);
}