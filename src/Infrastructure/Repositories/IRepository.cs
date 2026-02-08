using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> SaveChangesAsync();
    
    // Paging methods
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetCountAsync();
}
