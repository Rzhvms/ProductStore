using System.Text.Json.Nodes;

namespace Application.UseCases.Product.Dto.Request;

/// <summary>
/// дто для создание продукта
/// </summary>
public record CreateProductRequest
{
    public string Name { get; init; }
    public Guid? ProviderId { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public Guid CategoryId { get; init; }
    public JsonObject? Characteristics { get; init; }
}