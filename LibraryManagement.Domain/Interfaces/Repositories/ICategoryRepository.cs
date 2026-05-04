using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Repositories;
//permitirá consultar categorías con sus libros y validar que no existan categorías repetidas.
public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoriesWithBooksAsync();

    Task<Category?> GetCategoryWithBooksByIdAsync(int id);

    Task<bool> ExistsByNameAsync(string name, int? excludeCategoryId = null);
}