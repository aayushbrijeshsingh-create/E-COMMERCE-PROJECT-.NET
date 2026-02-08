using ECommerce.Business.Logic.Services;
using ECommerce.Models;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userId!);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PaymentResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentDto dto)
    {
        var result = await _paymentService.ProcessPaymentAsync(dto);
        if (result.Success)
            return Ok(ApiResponse<PaymentResultDto>.Ok(result, "Payment processed successfully"));
        return BadRequest(ApiResponse<PaymentResultDto>.Error(result.Message));
    }

    [HttpGet("order/{orderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentByOrder(Guid orderId)
    {
        var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);
        return Ok(ApiResponse<PaymentDto>.Ok(payment!));
    }
}
