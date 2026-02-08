using AutoMapper;
using ECommerce.Models.DTOs;
using ECommerce.Models.Exceptions;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories;

namespace ECommerce.Business.Logic.Services.Implementation;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<PaymentResultDto> ProcessPaymentAsync(CreatePaymentDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(dto.OrderId);
        if (order == null)
            throw new NotFoundException("Order", dto.OrderId.ToString());
        
        // Simulate payment processing
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = dto.OrderId,
            Provider = dto.Provider,
            Status = PaymentStatus.Captured,
            Amount = order.TotalAmount,
            CreatedAt = DateTime.UtcNow
        };
        
        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();
        
        return new PaymentResultDto
        {
            Success = true,
            PaymentId = payment.Id,
            Message = "Payment processed successfully"
        };
    }

    public async Task<PaymentDto?> GetPaymentByOrderIdAsync(Guid orderId)
    {
        var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
        return payment != null ? _mapper.Map<PaymentDto>(payment) : null;
    }
}
