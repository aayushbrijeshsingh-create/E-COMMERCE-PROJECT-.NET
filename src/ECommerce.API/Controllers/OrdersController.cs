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
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId!);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<OrderSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyOrders()
    {
        var orders = await _orderService.GetOrdersByCustomerIdAsync(GetCurrentUserId());
        return Ok(ApiResponse<List<OrderSummaryDto>>.Ok(orders));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id, GetCurrentUserId());
        return Ok(ApiResponse<OrderDto>.Ok(order));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrder()
    {
        var order = await _orderService.CreateOrderFromCartAsync(GetCurrentUserId());
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, 
            ApiResponse<OrderDto>.Ok(order, "Order created successfully"));
    }
}

[ApiController]
[Route("api/[controller]")]
public class AdminOrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public AdminOrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<List<OrderSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(ApiResponse<List<OrderSummaryDto>>.Ok(orders));
    }

    [HttpPut("{id:guid}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string status)
    {
        await _orderService.UpdateOrderStatusAsync(id, status);
        return NoContent();
    }
}
