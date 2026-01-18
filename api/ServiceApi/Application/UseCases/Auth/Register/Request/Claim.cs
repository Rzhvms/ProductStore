using System.Text.Json.Serialization;

namespace Application.UseCases.Auth.Register.Request;

/// <summary>
/// Представляет право или роль пользователя.
/// </summary>
public record Claim
{
    /// <summary>
    /// Тип права/роли
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; }

    /// <summary>
    /// Значение права/роли
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; init; }
}