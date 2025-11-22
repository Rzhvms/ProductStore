using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.User.ChangeUserPassword.Request;

/// <summary>
/// Входная модель изменения пароль
/// </summary>
public record ChangeUserPasswordRequest
{
    /// <summary>
    /// Новый пароль
    /// </summary>
    [JsonPropertyName("newPassword")]
    [Required]
    public required string NewPassword { get; init; }
    
    /// <summary>
    /// Старый пароль
    /// </summary>
    [JsonPropertyName("oldPassword")]
    [Required]
    public required string OldPassword { get; init; }
}