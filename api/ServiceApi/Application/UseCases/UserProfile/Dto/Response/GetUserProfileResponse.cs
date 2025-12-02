using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.UserProfile.Dto.Response;

/// <summary>
/// Выходная модель на получение данных о профиле пользователя
/// </summary>
public record GetUserProfileResponse
{
    /// <summary>
    /// Имя
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate { get; init; }
    
    /// <summary>
    /// Пол
    /// </summary>
    [JsonPropertyName("gender")]
    public string? Gender { get; init; }
    
    /// <summary>
    /// Почта
    /// </summary>
    [JsonPropertyName("email")]
    [EmailAddress]
    public string? Email { get; init; }
    
    /// <summary>
    /// Телефон
    /// </summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; init; }
    
    /// <summary>
    /// О себе
    /// </summary>
    [JsonPropertyName("about")]
    public string? About { get; init; }
    
    // /// <summary>
    // /// Путь до аватара
    // /// </summary>
    // public string? AvatarUrl { get; init; }
}