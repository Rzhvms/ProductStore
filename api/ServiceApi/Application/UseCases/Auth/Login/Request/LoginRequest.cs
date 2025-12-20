using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Auth.Login.Request;

/// <summary>
/// Входная модель для авторизации пользователя
/// </summary>
public record LoginRequest
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public required string Email { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [MinLength(8)]
    public required string Password { get; set; }
}