using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ECommerceDbContext context) : base(context)
    {
    }

    public async Task<Cart?> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<Cart?> GetByCustomerIdWithItemsAsync(Guid customerId)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }
}
