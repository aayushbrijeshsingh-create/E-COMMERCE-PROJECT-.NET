using ECommerce.Business.Logic.Services;
using ECommerce.Models;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId!);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _cartService.GetCartByCustomerIdAsync(GetCurrentUserId());
        return Ok(ApiResponse<CartDto>.Ok(cart));
    }

    [HttpPost("items")]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto)
    {
        var cart = await _cartService.AddItemToCartAsync(GetCurrentUserId(), dto);
        return Ok(ApiResponse<CartDto>.Ok(cart, "Item added to cart"));
    }

    [HttpPut("items/{itemId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CartDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateCartItemDto dto)
    {
        dto.ItemId = itemId;
        var cart = await _cartService.UpdateCartItemAsync(GetCurrentUserId(), dto);
        return Ok(ApiResponse<CartDto>.Ok(cart, "Cart item updated"));
    }

    [HttpDelete("items/{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveItem(Guid itemId)
    {
        await _cartService.RemoveCartItemAsync(GetCurrentUserId(), itemId);
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync(GetCurrentUserId());
        return NoContent();
    }
}
