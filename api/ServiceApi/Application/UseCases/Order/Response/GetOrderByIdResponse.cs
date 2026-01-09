using System.Text.Json.Nodes;
using Domain.Order;

namespace Application.UseCases.Order.Response;

/// <summary>
/// Выходная модель получения заказа
/// </summary>
public record GetOrderByIdResponse
{
    /// <summary>
    /// Айди заказа
    /// </summary>
    public Guid? OrderId { get; init; }
    
    /// <summary>
    /// Идентификатор клиента покупателя (userId)
    /// </summary>
    public Guid? CustomerId { get; init; }
    
    /// <summary>
    /// Состояние заказа
    /// </summary>
    public OrderStateEnum? State { get; init; }
    
    /// <summary>
    /// Идентификатор доставки
    /// </summary>
    public Guid? DeliveryId { get; init; }
    
    /// <summary>
    /// Сумма заказа
    /// </summary>
    public decimal? TotalSum { get; init; }
    
    /// <summary>
    /// Дата и время заказа
    /// </summary>
    public DateTime? OrderDate { get; init; }
    
    /// <summary>
    /// Информация о продуктах из заказа
    /// </summary>
    public JsonArray? ProductListInOrder { get; init; }
}