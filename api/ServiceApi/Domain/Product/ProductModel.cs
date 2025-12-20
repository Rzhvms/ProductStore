using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace Domain.Product;

/// <summary>
/// Продукт
/// </summary>
public record ProductModel
{
    /// <summary>
    /// Айди продукта
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название продукта
    /// </summary>
    [MaxLength(150)]
    [Required]
    public required string Name { get; init; }
    
    /// <summary>
    /// Id поставщика продукта
    /// </summary>
    public Guid? ProviderId { get; init; }
    
    /// <summary>
    /// Категория
    /// </summary>
    public Guid? CategoryId { get; init; }
    
    /// <summary>
    /// Описание продукта
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; init; }
    
    /// <summary>
    /// Цена
    /// </summary>
    [Required]
    [Range(0, double.MaxValue)]
    public required decimal Price { get; init; }
    
    /// <summary>
    /// Количество
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public required int Quantity { get; init; }
    
    /// <summary>
    /// Характеристики товара
    /// </summary>
    public JsonObject? Characteristics { get; init; }
}