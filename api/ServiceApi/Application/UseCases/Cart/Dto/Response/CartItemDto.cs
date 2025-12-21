namespace Application.UseCases.Cart.Dto.Response;

/// <summary>
/// Элемент корзины
/// </summary>
public record CartItemDto
{
    /// <summary>
    /// Айди продукта
    /// </summary>
    public Guid ProductId { get; init; }
    
    /// <summary>
    /// Название продукта
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Количество продуктов
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Актуальная цена продукта
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Общая сумма по этому продукту (Price * Quantity)
    /// </summary>
    public decimal Total => Price * Quantity;
}