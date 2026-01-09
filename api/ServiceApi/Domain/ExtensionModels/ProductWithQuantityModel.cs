namespace Domain.ExtensionModels;

/// <summary>
/// Соотношение Id продукта и количества в заказе
/// </summary>
public record ProductWithQuantityModel
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}