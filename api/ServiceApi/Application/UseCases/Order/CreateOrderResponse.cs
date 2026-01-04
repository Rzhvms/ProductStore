namespace Application.UseCases.Order;

/// <summary>
/// Выходная модель для создания заказа
/// </summary>
public record CreateOrderResponse
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public Guid OrderId { get; init; }
}