using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ECommerceDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(name) && p.IsActive)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetProductBySkuAsync(string sku)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Sku == sku && p.IsActive);
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
    {
        return await _context.Products
            .Where(p => p.StockQuantity <= threshold && p.IsActive)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, Guid? categoryId)
    {
        var query = _context.Products
            .Where(p => p.IsActive)
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync(string? searchTerm, Guid? categoryId)
    {
        var query = _context.Products
            .Where(p => p.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        return await query.CountAsync();
    }
}
