using System.Text.Json.Nodes;

namespace Domain.ExtensionModels;

public record ResultProductModel
{
    public Guid Id { get; init; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? ProviderId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid CategoryId { get; set; }
    public JsonObject? Characteristics { get; init; }
}