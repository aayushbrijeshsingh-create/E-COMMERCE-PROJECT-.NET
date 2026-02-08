namespace Infrastructure.Data.Entities;

public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int LowStockThreshold { get; set; } = 10;
    public DateTime? LastRestockedAt { get; set; }
}
