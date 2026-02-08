namespace Infrastructure.Data.Entities;

public class CouponUsage : BaseEntity
{
    public Guid CouponId { get; set; }
    public Coupon? Coupon { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public Customer? Customer { get; set; }
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
}
