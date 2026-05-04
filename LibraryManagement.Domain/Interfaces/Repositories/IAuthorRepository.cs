using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;

//permitirá consultar autores con sus libros asociados y validar nombres duplicados.
public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<IEnumerable<Author>> GetAuthorsWithBooksAsync();

    Task<Author?> GetAuthorWithBooksByIdAsync(int id);

    Task<bool> ExistsByFullNameAsync(string firstName, string lastName, int? excludeAuthorId = null);
}