using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ECommerceDbContext context) : base(context)
    {
    }

    public async Task<List<Review>> GetByProductIdAsync(Guid productId)
    {
        return await _context.Reviews
            .Where(r => r.ProductId == productId && r.IsActive)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Review>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Reviews
            .Where(r => r.UserId == userId && r.IsActive)
            .Include(r => r.Product)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
}
