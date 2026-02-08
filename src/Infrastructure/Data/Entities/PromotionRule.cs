namespace Infrastructure.Data.Entities;

public enum RuleType
{
    Category = 1,
    Product = 2,
    CustomerSegment = 3
}

public class PromotionRule : BaseEntity
{
    public RuleType RuleType { get; set; }
    public string RuleExpression { get; set; } = string.Empty; // JSON
}
