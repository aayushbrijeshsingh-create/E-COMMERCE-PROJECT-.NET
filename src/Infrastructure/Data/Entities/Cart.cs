namespace Infrastructure.Data.Entities;

public class Cart : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public List<CartItem> Items { get; set; } = new();
}

public class CartItem : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
}
