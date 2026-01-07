using Domain.Payment;

namespace Application.Ports.Services.Payment;

/// <summary>
/// Сервис оплаты
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Создать платеж
    /// </summary>
    Task<PaymentModel> CreatePaymentAsync(Guid orderId, Guid userId, decimal amount);
    
    /// <summary>
    /// Установить успешный статус оплаты
    /// </summary>
    Task SetPaidAsync(Guid paymentId);
    
    /// <summary>
    /// Установить статус отмены оплаты
    /// </summary>
    Task SetCanceledAsync(Guid paymentId);
}