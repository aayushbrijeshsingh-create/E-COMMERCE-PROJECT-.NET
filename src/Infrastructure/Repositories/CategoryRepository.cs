using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ECommerceDbContext context) : base(context)
    {
    }

    public async Task<List<Category>> GetParentCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.ParentId == null && c.IsActive)
            .Include(c => c.SubCategories)
            .ToListAsync();
    }
}
