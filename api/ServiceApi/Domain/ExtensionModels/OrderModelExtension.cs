using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;
using Domain.Order;

namespace Domain.ExtensionModels;

/// <summary>
/// Внутренняя вспомогательная модель для заказа
/// </summary>
public record OrderModelExtension
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
    [Range(0, double.MaxValue)]
    public decimal? TotalSum { get; init; }
    
    /// <summary>
    /// Дата и время заказа
    /// </summary>
    public DateTime? OrderDate { get; init; }
    
    /// <summary>
    /// Список идентификаторов продуктов из корзины
    /// </summary>
    public IEnumerable<ProductWithQuantityModel>? ProductIdWithQuantityList { get; init; }
    
    /// <summary>
    /// Информация о продуктах из заказа
    /// </summary>
    [Column("ProductListInOrder")]
    public JsonArray? ProductListInOrder { get; init; }
}