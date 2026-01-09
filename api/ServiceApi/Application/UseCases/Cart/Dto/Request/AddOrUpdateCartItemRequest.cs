using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Cart.Dto.Request;

/// <summary>
/// Запрос на добавление/обновление элемента корзины
/// </summary>
public record AddOrUpdateCartItemRequest
{
    /// <summary>
    /// Id продукта
    /// </summary>
    [Required]
    public required Guid ProductId { get; init; }

    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; init; }
}