using System.Text.Json.Nodes;

namespace Domain.ExtensionModels;

/// <summary>
/// Вспомогательная сущность для получения продукта
/// </summary>
public record ResultProductModel
{
    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название продукта
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Описание продукта
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Идентификатор поставщика
    /// </summary>
    public Guid? ProviderId { get; init; }
    
    /// <summary>
    /// Цена
    /// </summary>
    public decimal Price { get; init; }
    
    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; init; }
    
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid CategoryId { get; init; }
    
    /// <summary>
    /// Характеристики
    /// </summary>
    public JsonObject? Characteristics { get; init; }
}