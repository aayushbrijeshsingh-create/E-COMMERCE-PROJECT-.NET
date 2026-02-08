using Infrastructure.Data.Entities;

namespace Infrastructure.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetParentCategoriesAsync();
}
