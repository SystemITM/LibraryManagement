using LibraryManagement.DataAccess.Context;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.DataAccess.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await GetCategoriesWithBooksAsync();
    }

    public override async Task<Category?> GetByIdAsync(int id)
    {
        return await GetCategoryWithBooksByIdAsync(id);
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithBooksAsync()
    {
        return await _context.Categories
            .Include(category => category.Books)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryWithBooksByIdAsync(int id)
    {
        return await _context.Categories
            .Include(category => category.Books)
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id);
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeCategoryId = null)
    {
        var normalizedName = name.Trim();

        return await _context.Categories
            .AnyAsync(category =>
                category.Name == normalizedName &&
                (!excludeCategoryId.HasValue || category.Id != excludeCategoryId.Value));
    }
}