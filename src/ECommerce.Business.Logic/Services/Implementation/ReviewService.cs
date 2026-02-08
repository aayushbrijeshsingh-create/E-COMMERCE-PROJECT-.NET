using AutoMapper;
using ECommerce.Models.DTOs;
using ECommerce.Models.Exceptions;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories;

namespace ECommerce.Business.Logic.Services.Implementation;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ReviewService(
        IReviewRepository reviewRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ReviewDto>> GetReviewsByProductIdAsync(Guid productId)
    {
        var reviews = await _reviewRepository.GetByProductIdAsync(productId);
        return _mapper.Map<List<ReviewDto>>(reviews);
    }

    public async Task<ReviewDto> CreateReviewAsync(Guid userId, CreateReviewDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.ProductId);
        if (product == null)
            throw new NotFoundException("Product", dto.ProductId.ToString());
        
        var review = new Review
        {
            Id = Guid.NewGuid(),
            ProductId = dto.ProductId,
            UserId = userId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow
        };
        
        await _reviewRepository.AddAsync(review);
        await _reviewRepository.SaveChangesAsync();
        
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task DeleteReviewAsync(Guid reviewId, Guid userId)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
            throw new NotFoundException("Review", reviewId.ToString());
        
        if (review.UserId != userId)
            throw new ForbiddenException("You can only delete your own reviews");
        
        _reviewRepository.Delete(review);
        await _reviewRepository.SaveChangesAsync();
    }

    public async Task<ProductReviewSummaryDto> GetProductReviewSummaryAsync(Guid productId)
    {
        var reviews = await _reviewRepository.GetByProductIdAsync(productId);
        
        return new ProductReviewSummaryDto
        {
            ProductId = productId,
            AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0,
            TotalReviews = reviews.Count(),
            RecentReviews = _mapper.Map<List<ReviewDto>>(reviews.OrderByDescending(r => r.CreatedAt).Take(5))
        };
    }
}
