namespace Infrastructure.Data.Entities;

public enum DiscountType
{
    Percentage = 1,
    Fixed = 2
}

public class Coupon : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public int CurrentUsageCount { get; set; } = 0;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
}
