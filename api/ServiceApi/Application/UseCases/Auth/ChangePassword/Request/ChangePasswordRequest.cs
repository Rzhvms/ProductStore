using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Auth.ChangePassword.Request;

/// <summary>
/// Модель изменения пароля
/// </summary>
public record ChangePasswordRequest
{
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    public required string Password { get; init; }
}