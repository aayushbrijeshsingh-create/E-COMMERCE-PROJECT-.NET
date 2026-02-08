using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByCustomerIdAsync(string customerId);
    Task<Cart?> GetByCustomerIdWithItemsAsync(string customerId);
}
