using Domain.Payment;

namespace Application.UseCases.Payment;

/// <summary>
/// Управление платежами
/// </summary>
public interface IPaymentUseCase
{
    /// <summary>
    /// Произвести оплату
    /// </summary>
    Task PayAsync(Guid paymentId);
    
    /// <summary>
    /// Отменить оплату
    /// </summary>
    Task CancelAsync(Guid paymentId);
    
    /// <summary>
    /// Получить платеж
    /// </summary>
    Task<PaymentModel?> GetPaymentByIdAsync(Guid paymentId);
}