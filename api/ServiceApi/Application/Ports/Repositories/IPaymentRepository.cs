using Domain.Payment;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для работы с платежами
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Создание платежа
    /// </summary>
    Task CreateAsync(PaymentModel payment);
    
    /// <summary>
    /// Обновление статуса платежа
    /// </summary>
    Task UpdateStatusAsync(Guid paymentId, PaymentStatusEnum status);
    
    /// <summary>
    /// Получение платежа по идентификатору
    /// </summary>
    Task<PaymentModel?> GetByIdAsync(Guid paymentId);
}