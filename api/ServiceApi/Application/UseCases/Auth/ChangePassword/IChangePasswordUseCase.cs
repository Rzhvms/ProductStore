using Application.UseCases.Auth.ChangePassword.Request;
using Application.UseCases.User.ChangeUserPassword.Response;

namespace Application.UseCases.Auth.ChangePassword;

/// <summary>
/// Изменение пароля
/// </summary>
public interface IChangePasswordUseCase
{
    /// <summary>
    /// Основная логика обработки
    /// </summary>
    Task ExecuteAsync(Guid id, ChangePasswordRequest request);
}