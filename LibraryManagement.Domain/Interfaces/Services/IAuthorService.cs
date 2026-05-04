using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;
//Este servicio manejará la lógica de autores.
public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAsync();

    Task<Author?> GetByIdAsync(int id);

    Task<Author> CreateAsync(Author author);

    Task<Author> UpdateAsync(int id, Author author);

    Task<bool> DeleteAsync(int id);
}