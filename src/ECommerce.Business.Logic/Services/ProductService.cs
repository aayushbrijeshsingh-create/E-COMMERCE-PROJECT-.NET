using ECommerce.Models;
using ECommerce.Models.DTOs;
using ECommerce.Models.Exceptions;

namespace ECommerce.Business.Logic.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<PagedApiResponse<List<ProductDto>>> GetProductsAsync(int pageNumber, int pageSize, string? searchTerm, Guid? categoryId);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task UpdateProductAsync(UpdateProductDto dto);
    Task DeleteProductAsync(Guid id);
    Task UpdateInventoryAsync(Guid productId, int quantity);
}

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task UpdateCategoryAsync(UpdateCategoryDto dto);
    Task DeleteCategoryAsync(Guid id);
}
