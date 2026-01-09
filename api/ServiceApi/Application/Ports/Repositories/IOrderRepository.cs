using Domain.ExtensionModels;
using Domain.Order;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для управления заказами
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Получить заказ по идентификатору
    /// </summary>
    /// <returns></returns>
    Task<OrderModelExtension?> GetOrderByIdAsync(Guid orderId);
    
    /// <summary>
    /// Получить список заказов
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<OrderModel>> GetOrderListAsync(int pageNumber, int pageSize);
    
    /// <summary>
    /// Создать заказ
    /// </summary>
    Task<Guid> CreateOrderAsync(OrderModelExtension orderModel);

    /// <summary>
    /// Изменить статус заказа
    /// </summary>
    Task ChangeOrderStatusAsync(Guid orderId, OrderStateEnum orderState);
}