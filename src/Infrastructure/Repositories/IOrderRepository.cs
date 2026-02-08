using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
    Task<IEnumerable<Order>> GetByCustomerIdAsync(string customerId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<Order?> GetOrderWithItemsAsync(Guid orderId);
}
