using Domain.Order;

namespace Application.UseCases.Order.Request;

/// <summary>
/// Входная модель на изменение состояния заказа
/// </summary>
public record ChangeOrderStatusRequest
{
    public OrderStateEnum State { get; init; }
}