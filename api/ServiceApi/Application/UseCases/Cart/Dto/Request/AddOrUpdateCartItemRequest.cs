namespace Application.UseCases.Cart.Dto.Request;

/// <summary>
/// Запрос на добавление/обновление элемента корзины
/// </summary>
public record AddOrUpdateCartItemRequest
{
    /// <summary>
    /// Id продукта
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; init; }
}