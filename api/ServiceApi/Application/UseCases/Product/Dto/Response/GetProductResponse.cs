using System.Text.Json.Nodes;

namespace Application.UseCases.Product.Dto.Response;

/// <summary>
/// Выходная модель для получения продукта на клиентской части
/// </summary>
public record GetProductResponse
{
    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название продукта
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Описание продукта
    /// </summary>
    public string Description { get; init; }
    
    /// <summary>
    /// Цена продукта
    /// </summary>
    public decimal Price { get; init; }
    
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid CategoryId { get; init; }
    
    /// <summary>
    /// Характеристики
    /// </summary>
    public JsonObject Characteristics { get; init; }
}