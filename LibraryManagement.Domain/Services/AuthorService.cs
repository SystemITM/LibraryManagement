using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _authorRepository.GetAllAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _authorRepository.GetByIdAsync(id);
    }

    public async Task<Author> CreateAsync(Author author)
    {
        ValidateAuthor(author);

        author.FirstName = author.FirstName.Trim();
        author.LastName = author.LastName.Trim();
        author.Biography = author.Biography?.Trim();

        if (await _authorRepository.ExistsByFullNameAsync(author.FirstName, author.LastName))
        {
            throw new InvalidOperationException("Ya existe un autor con el mismo nombre y apellido.");
        }

        return await _authorRepository.CreateAsync(author);
    }

    public async Task<Author> UpdateAsync(int id, Author author)
    {
        ValidateAuthor(author);

        var existingAuthor = await _authorRepository.GetByIdAsync(id);

        if (existingAuthor is null)
        {
            throw new KeyNotFoundException("El autor no existe.");
        }

        if (await _authorRepository.ExistsByFullNameAsync(author.FirstName, author.LastName, id))
        {
            throw new InvalidOperationException("Ya existe otro autor con el mismo nombre y apellido.");
        }

        existingAuthor.FirstName = author.FirstName.Trim();
        existingAuthor.LastName = author.LastName.Trim();
        existingAuthor.Biography = author.Biography?.Trim();

        return await _authorRepository.UpdateAsync(existingAuthor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _authorRepository.GetAuthorWithBooksByIdAsync(id);

        if (author is null)
        {
            return false;
        }

        if (author.BookAuthors.Any())
        {
            throw new InvalidOperationException("No se puede eliminar un autor que tiene libros asociados.");
        }

        return await _authorRepository.DeleteAsync(id);
    }

    private static void ValidateAuthor(Author author)
    {
        if (string.IsNullOrWhiteSpace(author.FirstName))
        {
            throw new InvalidOperationException("El nombre del autor es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(author.LastName))
        {
            throw new InvalidOperationException("El apellido del autor es obligatorio.");
        }

        if (author.FirstName.Length > 100)
        {
            throw new InvalidOperationException("El nombre del autor no puede superar los 100 caracteres.");
        }

        if (author.LastName.Length > 100)
        {
            throw new InvalidOperationException("El apellido del autor no puede superar los 100 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(author.Biography) && author.Biography.Length > 500)
        {
            throw new InvalidOperationException("La biografía del autor no puede superar los 500 caracteres.");
        }
    }
}