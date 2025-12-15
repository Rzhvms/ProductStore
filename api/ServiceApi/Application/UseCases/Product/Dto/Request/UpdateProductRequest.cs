using System.Text.Json.Nodes;

namespace Application.UseCases.Product.Dto.Request;

public record UpdateProductRequest
{
    /// <summary>
    /// Название продукта
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Id поставщика продукта
    /// </summary>
    public Guid? ProviderId { get; init; }
    
    /// <summary>
    /// Категория
    /// </summary>
    public Guid CategoryId { get; init; }
    
    /// <summary>
    /// Описание продукта
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Цена
    /// </summary>
    public decimal Price { get; init; }
    
    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; init; }
    
    /// <summary>
    /// Характеристики товара
    /// </summary>
    public JsonObject? Characteristics { get; init; }
}