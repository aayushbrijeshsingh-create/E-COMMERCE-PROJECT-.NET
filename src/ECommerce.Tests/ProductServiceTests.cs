using AutoMapper;
using ECommerce.Business.Logic.MappingProfiles;
using ECommerce.Business.Logic.Services;
using ECommerce.Business.Logic.Services.Implementation;
using ECommerce.Models.DTOs;
using ECommerce.Models.Exceptions;
using FluentAssertions;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories;
using Moq;
using Xunit;

namespace ECommerce.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _productService = new ProductService(
            _productRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _mapper);
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsProductDtos()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Product 1", Price = 10.99m },
            new() { Id = Guid.NewGuid(), Name = "Product 2", Price = 20.99m }
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Product 1");
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductExists_ReturnsProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Test Product",
            Price = 15.99m
        };

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _productService.GetProductByIdAsync(productId));
    }

    [Fact]
    public async Task CreateProductAsync_CreatesAndReturnsProductDto()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "New Product",
            Description = "Description",
            Price = 25.99m,
            CategoryId = Guid.NewGuid(),
            StockQuantity = 100
        };

        _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);
        _productRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _productService.CreateProductAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("New Product");
        result.Price.Should().Be(25.99m);
    }
}
