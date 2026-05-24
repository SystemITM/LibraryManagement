using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAuthorRepository _authorRepository;

    public BookService(
        IBookRepository bookRepository,
        ICategoryRepository categoryRepository,
        IAuthorRepository authorRepository)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _authorRepository = authorRepository;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _bookRepository.GetAllAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<Book> CreateAsync(Book book, IEnumerable<int> authorIds)
    {
        var normalizedAuthorIds = NormalizeAuthorIds(authorIds);

        book.AvailableCopies = book.TotalCopies;

        ValidateBook(book);
        await ValidateRelationsAsync(book.CategoryId, normalizedAuthorIds);

        book.Title = book.Title.Trim();
        book.Isbn = book.Isbn.Trim();

        if (await _bookRepository.ExistsByIsbnAsync(book.Isbn))
        {
            throw new InvalidOperationException("Ya existe un libro con el mismo ISBN.");
        }

        book.BookAuthors = normalizedAuthorIds
    .Select(authorId => new BookAuthor { AuthorId = authorId })
    .ToList();

        var createdBook = await _bookRepository.CreateAsync(book);

        var createdBookWithDetails = await _bookRepository.GetByIdAsync(createdBook.Id);

        return createdBookWithDetails!;
    }

    public async Task<Book> UpdateAsync(int id, Book book, IEnumerable<int> authorIds)
    {
        var normalizedAuthorIds = NormalizeAuthorIds(authorIds);

        ValidateBook(book);
        await ValidateRelationsAsync(book.CategoryId, normalizedAuthorIds);

        var existingBook = await _bookRepository.GetBookForUpdateAsync(id);

        if (existingBook is null)
        {
            throw new KeyNotFoundException("El libro no existe.");
        }

        if (await _bookRepository.ExistsByIsbnAsync(book.Isbn, id))
        {
            throw new InvalidOperationException("Ya existe otro libro con el mismo ISBN.");
        }

        existingBook.Title = book.Title.Trim();
        existingBook.Isbn = book.Isbn.Trim();
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.TotalCopies = book.TotalCopies;
        existingBook.AvailableCopies = book.AvailableCopies;
        existingBook.CategoryId = book.CategoryId;

        await _bookRepository.UpdateAsync(existingBook);
        await _bookRepository.UpdateBookAuthorsAsync(existingBook.Id, normalizedAuthorIds);

        var updatedBook = await _bookRepository.GetByIdAsync(existingBook.Id);

        return updatedBook!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _bookRepository.GetBookWithDetailsByIdAsync(id);

        if (book is null)
        {
            return false;
        }

        if (book.Loans.Any())
        {
            throw new InvalidOperationException("No se puede eliminar un libro que tiene préstamos registrados.");
        }

        return await _bookRepository.DeleteAsync(id);
    }

    private static void ValidateBook(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
        {
            throw new InvalidOperationException("El título del libro es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(book.Isbn))
        {
            throw new InvalidOperationException("El ISBN del libro es obligatorio.");
        }

        if (book.PublicationYear < 1000 || book.PublicationYear > DateTime.Now.Year)
        {
            throw new InvalidOperationException("El año de publicación no es válido.");
        }

        if (book.TotalCopies <= 0)
        {
            throw new InvalidOperationException("El número total de copias debe ser mayor a cero.");
        }

        if (book.AvailableCopies < 0)
        {
            throw new InvalidOperationException("El número de copias disponibles no puede ser negativo.");
        }

        if (book.AvailableCopies > book.TotalCopies)
        {
            throw new InvalidOperationException("Las copias disponibles no pueden superar el total de copias.");
        }
    }

    private static List<int> NormalizeAuthorIds(IEnumerable<int> authorIds)
    {
        var normalizedAuthorIds = authorIds
            .Where(authorId => authorId > 0)
            .Distinct()
            .ToList();

        if (!normalizedAuthorIds.Any())
        {
            throw new InvalidOperationException("El libro debe tener al menos un autor asociado.");
        }

        return normalizedAuthorIds;
    }

    private async Task ValidateRelationsAsync(int categoryId, IEnumerable<int> authorIds)
    {
        if (!await _categoryRepository.ExistsAsync(categoryId))
        {
            throw new InvalidOperationException("La categoría asociada al libro no existe.");
        }

        foreach (var authorId in authorIds)
        {
            if (!await _authorRepository.ExistsAsync(authorId))
            {
                throw new InvalidOperationException($"El autor con Id {authorId} no existe.");
            }
        }
    }
}