using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book?> GetByIdAsync(int id);

    Task<Book> CreateAsync(Book book, IEnumerable<int> authorIds);

    Task<Book> UpdateAsync(int id, Book book, IEnumerable<int> authorIds);

    Task<bool> DeleteAsync(int id);
}