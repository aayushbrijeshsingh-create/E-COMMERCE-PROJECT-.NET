using ECommerce.Models.DTOs;

namespace ECommerce.Business.Logic.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    Task<bool> ValidateTokenAsync(string token);
    Task<UserDto?> GetUserByIdAsync(Guid userId);
    Task<UserDto?> GetUserByEmailAsync(string email);
}

public interface IReviewService
{
    Task<List<ReviewDto>> GetReviewsByProductIdAsync(Guid productId);
    Task<ReviewDto> CreateReviewAsync(Guid userId, CreateReviewDto dto);
    Task DeleteReviewAsync(Guid reviewId, Guid userId);
    Task<ProductReviewSummaryDto> GetProductReviewSummaryAsync(Guid productId);
}
