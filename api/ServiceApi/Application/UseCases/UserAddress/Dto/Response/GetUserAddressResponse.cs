using System.Text.Json.Serialization;

namespace Application.UseCases.UserAddress.Dto.Response;

/// <summary>
/// Выходная модель получения адреса пользователя
/// </summary>
public record GetUserAddressResponse
{
    /// <summary>
    /// id адреса
    /// </summary>
    [JsonPropertyName("id")]
    public Guid? Id { get; init; }
    
    /// <summary>
    /// id пользователя
    /// </summary>
    [JsonPropertyName("userId")]
    public Guid? UserId { get; init; }
    
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
    
    /// <summary>
    /// Этаж
    /// </summary>
    [JsonPropertyName("floor")]
    public int? Floor { get; init; }
    
    /// <summary>
    /// Подъезд
    /// </summary>
    [JsonPropertyName("entrance")]
    public string? Entrance { get; init; }
}