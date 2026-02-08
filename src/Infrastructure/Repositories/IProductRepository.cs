using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task<Product?> GetProductBySkuAsync(string sku);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
    
    // Custom paged methods with filters
    Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, Guid? categoryId);
    Task<int> GetCountAsync(string? searchTerm, Guid? categoryId);
}
