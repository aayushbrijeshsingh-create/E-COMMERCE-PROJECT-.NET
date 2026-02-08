namespace Infrastructure.Data.Entities;

public class InventoryReservation : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiresAt { get; set; }
}
