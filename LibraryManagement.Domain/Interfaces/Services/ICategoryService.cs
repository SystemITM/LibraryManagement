using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces.Services;

//Este servicio manejará las categorías de libros.
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);

    Task<Category> CreateAsync(Category category);

    Task<Category> UpdateAsync(int id, Category category);

    Task<bool> DeleteAsync(int id);
}