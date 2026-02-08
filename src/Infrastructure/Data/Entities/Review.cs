namespace Infrastructure.Data.Entities;

public class Review : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid UserId { get; set; }
    public Customer? User { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
