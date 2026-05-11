using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using LibraryManagement.Domain.Interfaces.Services;

namespace LibraryManagement.Domain.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        ValidateCategory(category);

        category.Name = category.Name.Trim();
        category.Description = category.Description?.Trim();

        if (await _categoryRepository.ExistsByNameAsync(category.Name))
        {
            throw new InvalidOperationException("Ya existe una categoría con el mismo nombre.");
        }

        return await _categoryRepository.CreateAsync(category);
    }

    public async Task<Category> UpdateAsync(int id, Category category)
    {
        ValidateCategory(category);

        var existingCategory = await _categoryRepository.GetByIdAsync(id);

        if (existingCategory is null)
        {
            throw new KeyNotFoundException("La categoría no existe.");
        }

        if (await _categoryRepository.ExistsByNameAsync(category.Name, id))
        {
            throw new InvalidOperationException("Ya existe otra categoría con el mismo nombre.");
        }

        existingCategory.Name = category.Name.Trim();
        existingCategory.Description = category.Description?.Trim();

        return await _categoryRepository.UpdateAsync(existingCategory);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetCategoryWithBooksByIdAsync(id);

        if (category is null)
        {
            return false;
        }

        if (category.Books.Any())
        {
            throw new InvalidOperationException("No se puede eliminar una categoría que tiene libros asociados.");
        }

        return await _categoryRepository.DeleteAsync(id);
    }

    private static void ValidateCategory(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new InvalidOperationException("El nombre de la categoría es obligatorio.");
        }

        if (category.Name.Length > 100)
        {
            throw new InvalidOperationException("El nombre de la categoría no puede superar los 100 caracteres.");
        }

        if (!string.IsNullOrWhiteSpace(category.Description) && category.Description.Length > 300)
        {
            throw new InvalidOperationException("La descripción de la categoría no puede superar los 300 caracteres.");
        }
    }
}