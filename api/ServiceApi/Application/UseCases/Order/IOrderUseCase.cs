using Application.UseCases.Order.Request;
using Application.UseCases.Order.Response;

namespace Application.UseCases.Order;

/// <summary>
/// Управление заказами
/// </summary>
public interface IOrderUseCase
{
    /// <summary>
    /// Получить заказ по идентификатору
    /// </summary>
    /// <returns></returns>
    Task<GetOrderByIdResponse> GetOrderByIdAsync(Guid orderId);
    
    /// <summary>
    /// Получить список заказов
    /// </summary>
    /// <returns></returns>
    Task<GetOrderListResponse> GetOrderListAsync(int pageNumber, int pageSize);
    
    /// <summary>
    /// Создать заказ
    /// </summary>
    Task<CreateOrderResponse> CreateOrderAsync(Guid userId);
    
    /// <summary>
    /// Изменить статус заказа
    /// </summary>
    Task ChangeOrderStatusAsync(Guid id, ChangeOrderStatusRequest request);
}