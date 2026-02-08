using ECommerce.Models.DTOs;

namespace ECommerce.Business.Logic.Services;

public interface IOrderService
{
    Task<List<OrderSummaryDto>> GetOrdersByCustomerIdAsync(Guid customerId);
    Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid customerId);
    Task<OrderDto> CreateOrderAsync(Guid customerId);
    Task<OrderDto> CreateOrderFromCartAsync(Guid customerId);
    Task UpdateOrderStatusAsync(Guid orderId, string status);
    Task<List<OrderSummaryDto>> GetAllOrdersAsync();
}

public interface IPaymentService
{
    Task<PaymentResultDto> ProcessPaymentAsync(CreatePaymentDto dto);
    Task<PaymentDto?> GetPaymentByOrderIdAsync(Guid orderId);
}
