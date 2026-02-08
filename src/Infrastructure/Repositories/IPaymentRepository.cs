using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment?> GetByOrderIdAsync(Guid orderId);
}
