using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface IReviewRepository : IRepository<Review>
{
    Task<List<Review>> GetByProductIdAsync(Guid productId);
    Task<List<Review>> GetByUserIdAsync(Guid userId);
}
