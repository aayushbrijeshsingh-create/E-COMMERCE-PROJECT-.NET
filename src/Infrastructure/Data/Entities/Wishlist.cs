namespace Infrastructure.Data.Entities;

public class Wishlist : BaseEntity
{
    public string CustomerId { get; set; } = string.Empty;
    public Customer? Customer { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}
