using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(Guid customerId);
    Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<Order?> GetOrderWithItemsAsync(Guid orderId);
}
