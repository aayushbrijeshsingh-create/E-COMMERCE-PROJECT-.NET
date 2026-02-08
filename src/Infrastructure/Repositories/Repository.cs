using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ECommerceDbContext _context;
    private readonly DbSet<T> _entities;

    public Repository(ECommerceDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _entities.Update(entity);
    }

    public void Delete(T entity)
    {
        entity.IsActive = false;
        _entities.Update(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _entities
            .Where(e => e.IsActive)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
        return await _entities.CountAsync(e => e.IsActive);
    }
}
