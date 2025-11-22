using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.User.GetUserInfo.Request;

/// <summary>
/// Входная модель по получению базовой информации о пользователе
/// </summary>
public record GetUserInfoRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [Required]
    public required Guid Id { get; init; }
}