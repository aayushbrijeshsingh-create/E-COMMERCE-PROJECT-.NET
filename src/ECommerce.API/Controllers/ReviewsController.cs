using ECommerce.Business.Logic.Services;
using ECommerce.Models;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("product/{productId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<List<ReviewDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductReviews(Guid productId)
    {
        var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
        return Ok(ApiResponse<List<ReviewDto>>.Ok(reviews));
    }

    [HttpGet("product/{productId:guid}/summary")]
    [ProducesResponseType(typeof(ApiResponse<ProductReviewSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductReviewSummary(Guid productId)
    {
        var summary = await _reviewService.GetProductReviewSummaryAsync(productId);
        return Ok(ApiResponse<ProductReviewSummaryDto>.Ok(summary));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var review = await _reviewService.CreateReviewAsync(userId, dto);
        return CreatedAtAction(nameof(GetProductReviews), new { productId = dto.ProductId }, 
            ApiResponse<ReviewDto>.Ok(review, "Review created successfully"));
    }

    [HttpDelete("{reviewId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _reviewService.DeleteReviewAsync(reviewId, userId);
        return NoContent();
    }
}
