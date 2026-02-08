using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    Task<int> CompleteAsync();
}
