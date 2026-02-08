using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

/// <summary>
/// Factory for creating DbContext instances at design time for EF Core migrations
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ECommerceDbContext>
{
    public ECommerceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ECommerceDbContext>();
        
        // Use the default connection string from appsettings.json
        optionsBuilder.UseSqlServer("Server=localhost;Database=ECommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
        
        return new ECommerceDbContext(optionsBuilder.Options);
    }
}
