using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByCustomerIdAsync(Guid customerId);
    Task<Cart?> GetByCustomerIdWithItemsAsync(Guid customerId);
}
