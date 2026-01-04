using Domain.Order;

namespace Application.UseCases.Order.Response;

/// <summary>
/// 
/// </summary>
public record GetOrderListResponse
{
    /// <summary>
    /// Список заказов
    /// </summary>
    public IEnumerable<OrderModel> OrderList { get; init; }
}