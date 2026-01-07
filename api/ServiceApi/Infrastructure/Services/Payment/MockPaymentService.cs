using Application.Configuration;
using Application.Ports.Repositories;
using Application.Ports.Services.Payment;
using Domain.Payment;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Payment;

/// <inheritdoc/>
public class MockPaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly AppOptions _options;
    
    public MockPaymentService(IPaymentRepository paymentRepository, IOptions<AppOptions> options)
    {
        _paymentRepository = paymentRepository;
        _options = options.Value;
    }

    /// <inheritdoc/>
    public async Task<PaymentModel> CreatePaymentAsync(Guid orderId, Guid userId, decimal amount)
    {
        var paymentId = Guid.NewGuid();

        var payment = new PaymentModel
        {
            Id = paymentId,
            OrderId = orderId,
            UserId = userId,
            Amount = amount,
            Currency = "RUB",
            PaymentMethod = PaymentMethodEnum.Card,
            Status = PaymentStatusEnum.Pending,
            PaymentSystemId = $"MOCK-{Guid.NewGuid():N}",
            PaymentUrl = $"{_options.PublicBaseUrl}/mock-payment.html?paymentId={paymentId}",
            IsRefunded = false,
            RefundAmount = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _paymentRepository.CreateAsync(payment);
        return payment;
    }

    /// <inheritdoc/>
    public Task SetPaidAsync(Guid paymentId) =>
        _paymentRepository.UpdateStatusAsync(paymentId, PaymentStatusEnum.Paid);

    /// <inheritdoc/>
    public Task SetCanceledAsync(Guid paymentId) =>
        _paymentRepository.UpdateStatusAsync(paymentId, PaymentStatusEnum.Canceled);
}