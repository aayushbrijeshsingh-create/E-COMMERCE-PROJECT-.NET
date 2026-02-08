using AutoMapper;
using ECommerce.Models.DTOs;
using ECommerce.Models.Exceptions;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories;

namespace ECommerce.Business.Logic.Services.Implementation;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<CartDto> GetCartByCustomerIdAsync(Guid customerId)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
        if (cart == null)
            return new CartDto { CustomerId = customerId };
        
        return MapToCartDto(cart);
    }

    public async Task<CartDto> AddItemToCartAsync(Guid customerId, AddCartItemDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId);
        if (product == null)
            throw new NotFoundException("Product", dto.ProductId.ToString());
        
        if (product.StockQuantity < dto.Quantity)
            throw new BadRequestException("Insufficient stock quantity");
        
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
        if (cart == null)
        {
            cart = new Cart { Id = Guid.NewGuid(), CustomerId = customerId };
            await _cartRepository.AddAsync(cart);
        }
        
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            });
        }
        
        await _cartRepository.SaveChangesAsync();
        return MapToCartDto(cart);
    }

    public async Task<CartDto> UpdateCartItemAsync(Guid customerId, UpdateCartItemDto dto)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
        if (cart == null)
            throw new NotFoundException("Cart not found");
        
        var item = cart.Items.FirstOrDefault(i => i.Id == dto.ItemId);
        if (item == null)
            throw new NotFoundException("Cart item", dto.ItemId.ToString());
        
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        if (product == null)
            throw new NotFoundException("Product", item.ProductId.ToString());
        
        if (product.StockQuantity < dto.Quantity)
            throw new BadRequestException("Insufficient stock quantity");
        
        item.Quantity = dto.Quantity;
        await _cartRepository.SaveChangesAsync();
        
        return MapToCartDto(cart);
    }

    public async Task RemoveCartItemAsync(Guid customerId, Guid itemId)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
        if (cart == null)
            throw new NotFoundException("Cart not found");
        
        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new NotFoundException("Cart item", itemId.ToString());
        
        cart.Items.Remove(item);
        await _cartRepository.SaveChangesAsync();
    }

    public async Task ClearCartAsync(Guid customerId)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
        if (cart == null)
            return;
        
        cart.Items.Clear();
        await _cartRepository.SaveChangesAsync();
    }

    private CartDto MapToCartDto(Cart cart)
    {
        var dto = _mapper.Map<CartDto>(cart);
        dto.SubTotal = cart.Items.Sum(i =>
        {
            var product = _productRepository.GetByIdAsync(i.ProductId).Result;
            return product != null ? product.Price * i.Quantity : 0;
        });
        return dto;
    }
}
