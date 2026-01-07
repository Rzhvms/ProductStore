using Application.Ports.Repositories;
using Application.Ports.Services.Payment;
using Domain.Payment;

namespace Application.UseCases.Payment;

/// <inheritdoc/>
internal class PaymentUseCase : IPaymentUseCase
{
    private readonly IPaymentService _paymentService;
    private readonly IPaymentRepository _paymentRepository;

    public PaymentUseCase(
        IPaymentService paymentService,
        IPaymentRepository paymentRepository)
    {
        _paymentService = paymentService;
        _paymentRepository = paymentRepository;
    }
    
    /// <inheritdoc/>
    public async Task PayAsync(Guid paymentId)
    {
        await _paymentService.SetPaidAsync(paymentId);
    }

    /// <inheritdoc/>
    public async Task CancelAsync(Guid paymentId)
    {
        await _paymentService.SetCanceledAsync(paymentId);
    }

    /// <inheritdoc/>
    public async Task<PaymentModel?> GetPaymentByIdAsync(Guid paymentId)
    {
        return await _paymentRepository.GetByIdAsync(paymentId);
    }
}