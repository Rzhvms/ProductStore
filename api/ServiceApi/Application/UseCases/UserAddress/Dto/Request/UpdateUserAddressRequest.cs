using System.Text.Json.Serialization;

namespace Application.UseCases.UserAddress.Dto.Request;

/// <summary>
/// Входная модель обновления адреса
/// </summary>
public record UpdateUserAddressRequest
{
    /// <summary>
    /// Страна
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; init; }
    
    /// <summary>
    /// Область
    /// </summary>
    [JsonPropertyName("region")]
    public string? Region { get; init; }
    
    /// <summary>
    /// Город
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; init; }
    
    /// <summary>
    /// Улица
    /// </summary>
    [JsonPropertyName("street")]
    public string? Street { get; init; }
    
    /// <summary>
    /// Номер дома
    /// </summary>
    [JsonPropertyName("house")]
    public string? House { get; init; }
    
    /// <summary>
    /// Номер квартиры
    /// </summary>
    [JsonPropertyName("apartment")]
    public string? Apartment { get; init; }
    
    /// <summary>
    /// Почтовый индекс
    /// </summary>
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; init; }
    
    /// <summary>
    /// Использовать по умолчанию?
    /// </summary>
    [JsonPropertyName("isDefault")]
    public bool? IsDefault { get; init; }
}