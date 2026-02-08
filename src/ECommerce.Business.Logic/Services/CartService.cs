using ECommerce.Models.DTOs;

namespace ECommerce.Business.Logic.Services;

public interface ICartService
{
    Task<CartDto> GetCartByCustomerIdAsync(Guid customerId);
    Task<CartDto> AddItemToCartAsync(Guid customerId, AddCartItemDto dto);
    Task<CartDto> UpdateCartItemAsync(Guid customerId, UpdateCartItemDto dto);
    Task RemoveCartItemAsync(Guid customerId, Guid itemId);
    Task ClearCartAsync(Guid customerId);
}
