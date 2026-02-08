namespace Infrastructure.Data.Entities;

public enum OrderStatus
{
    PendingPayment = 1,
    Paid = 2,
    Processing = 3,
    Shipped = 4,
    Completed = 5,
    Cancelled = 6,
    Refunded = 7
}

public class Order : BaseEntity
{
    public string CustomerId { get; set; } = string.Empty;
    public Customer? Customer { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;
    public decimal TotalAmount { get; set; }
    
    // Extended order properties
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal GrandTotal { get; set; }
    
    // Address relationships
    public Guid? BillingAddressId { get; set; }
    public Address? BillingAddress { get; set; }
    public Guid? ShippingAddressId { get; set; }
    public Address? ShippingAddress { get; set; }
    
    public string? Notes { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    
    // Alias for OrderItems
    public ICollection<OrderItem> Items => OrderItems;
}
