using AutoMapper;
using ECommerce.Models;
using ECommerce.Models.DTOs;
using ECommerce.Models.Exceptions;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories;

namespace ECommerce.Business.Logic.Services.Implementation;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<List<ProductDto>>(products) ?? new List<ProductDto>();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException("Product", id.ToString());
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<PagedApiResponse<List<ProductDto>>> GetProductsAsync(
        int pageNumber, int pageSize, string? searchTerm, Guid? categoryId)
    {
        var products = await _productRepository.GetPagedAsync(pageNumber, pageSize, searchTerm, categoryId);
        var totalCount = await _productRepository.GetCountAsync(searchTerm, categoryId);
        
        var productDtos = _mapper.Map<List<ProductDto>>(products);
        
        return PagedApiResponse<List<ProductDto>>.Ok(
            productDtos ?? new List<ProductDto>(), pageNumber, pageSize, totalCount);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto) ?? throw new InvalidOperationException("Failed to map product");
        product.Id = Guid.NewGuid();
        
        await _productRepository.AddAsync(product);
        return _mapper.Map<ProductDto>(product) ?? throw new InvalidOperationException("Failed to map product");
    }

    public async Task UpdateProductAsync(UpdateProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.Id);
        if (product == null)
            throw new NotFoundException("Product", dto.Id.ToString());
        
        _mapper.Map(dto, product);
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException("Product", id.ToString());
        
        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync();
    }

    public async Task UpdateInventoryAsync(Guid productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            throw new NotFoundException("Product", productId.ToString());
        
        product.StockQuantity = quantity;
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();
    }
}
