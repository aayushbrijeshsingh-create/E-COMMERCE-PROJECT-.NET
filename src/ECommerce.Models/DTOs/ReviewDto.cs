namespace ECommerce.Models.DTOs;

public class ReviewDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateReviewDto
{
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}

public class ProductReviewSummaryDto
{
    public Guid ProductId { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public List<ReviewDto> RecentReviews { get; set; } = new();
}
