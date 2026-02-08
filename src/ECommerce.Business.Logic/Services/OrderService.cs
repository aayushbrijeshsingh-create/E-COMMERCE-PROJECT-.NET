using ECommerce.Models.DTOs;

namespace ECommerce.Business.Logic.Services;

public interface IOrderService
{
    Task<List<OrderSummaryDto>> GetOrdersByCustomerIdAsync(string customerId);
    Task<OrderDto?> GetOrderByIdAsync(Guid orderId, string customerId);
    Task<OrderDto> CreateOrderAsync(string customerId);
    Task<OrderDto> CreateOrderFromCartAsync(string customerId);
    Task UpdateOrderStatusAsync(Guid orderId, string status);
    Task<List<OrderSummaryDto>> GetAllOrdersAsync();
}

public interface IPaymentService
{
    Task<PaymentResultDto> ProcessPaymentAsync(CreatePaymentDto dto);
    Task<PaymentDto?> GetPaymentByOrderIdAsync(Guid orderId);
}
