namespace Infrastructure.Data.Entities;

public class Wishlist : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}
