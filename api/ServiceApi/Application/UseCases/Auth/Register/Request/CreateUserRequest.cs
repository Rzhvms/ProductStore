using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Claim = Application.UseCases.Auth.CreateUser.Request.Claim;

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
    public required string Email { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [JsonPropertyName("password")]
    public required string Password { get; set; }
    
    /// <summary>
    /// Права
    /// </summary>
    [JsonPropertyName("claims")]
    [Required]
    public required IList<Claim> Claims { get; set; }
}