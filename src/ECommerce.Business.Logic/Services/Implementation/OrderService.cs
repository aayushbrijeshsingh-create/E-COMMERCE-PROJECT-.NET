using AutoMapper;
using ECommerce.Models.DTOs;
using ECommerce.Models.Exceptions;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories;

namespace ECommerce.Business.Logic.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderSummaryDto>> GetOrdersByCustomerIdAsync(Guid customerId)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
        return orders.Select(o => new OrderSummaryDto
        {
            Id = o.Id,
            CreatedAt = o.CreatedAt,
            Status = o.Status.ToString(),
            TotalAmount = o.TotalAmount,
            ItemCount = o.OrderItems.Count
        }).ToList();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid customerId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null || order.CustomerId != customerId)
            throw new NotFoundException("Order", orderId.ToString());
        
        return MapToOrderDto(order);
    }

    public async Task<OrderDto> CreateOrderAsync(Guid customerId)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Status = OrderStatus.PendingPayment,
            CreatedAt = DateTime.UtcNow
        };
        
        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();
        
        return MapToOrderDto(order);
    }

    public async Task<OrderDto> CreateOrderFromCartAsync(Guid customerId)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(customerId);
        if (cart == null || !cart.Items.Any())
            throw new BadRequestException("Cart is empty");
        
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Status = OrderStatus.PendingPayment,
            CreatedAt = DateTime.UtcNow,
            OrderItems = new List<OrderItem>()
        };
        
        foreach (var cartItem in cart.Items)
        {
            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
            if (product == null)
                throw new NotFoundException("Product", cartItem.ProductId.ToString());
            
            if (product.StockQuantity < cartItem.Quantity)
                throw new BadRequestException($"Insufficient stock for product: {product.Name}");
            
            order.OrderItems.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = product.Price
            });
            
            product.StockQuantity -= cartItem.Quantity;
        }
        
        order.TotalAmount = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);
        
        await _orderRepository.AddAsync(order);
        cart.Items.Clear();
        await _orderRepository.SaveChangesAsync();
        
        return MapToOrderDto(order);
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, string status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException("Order", orderId.ToString());
        
        if (Enum.TryParse<OrderStatus>(status, out var orderStatus))
        {
            order.Status = orderStatus;
        }
        else
        {
            throw new BadRequestException("Invalid order status");
        }
        
        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();
    }

    public async Task<List<OrderSummaryDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(o => new OrderSummaryDto
        {
            Id = o.Id,
            CreatedAt = o.CreatedAt,
            Status = o.Status.ToString(),
            TotalAmount = o.TotalAmount,
            ItemCount = o.OrderItems.Count
        }).ToList();
    }

    private OrderDto MapToOrderDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}
