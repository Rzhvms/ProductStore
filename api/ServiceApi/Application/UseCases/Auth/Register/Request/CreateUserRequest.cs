using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Claim = Application.UseCases.Auth.Register.Request.Claim;

namespace Application.UseCases.Auth.Register.Request;

/// <summary>
/// Входная модель создания пользователя
/// </summary>
public record CreateUserRequest
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    [EmailAddress]
    [MaxLength(50)]
    [Required]
    [JsonPropertyName("email")]
    public required string Email { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [MinLength(8)]
    [JsonPropertyName("password")]
    public required string Password { get; init; }
    
    /// <summary>
    /// Права
    /// </summary>
    [JsonPropertyName("claims")]
    [Required]
    public required IList<Claim> Claims { get; init; }
}