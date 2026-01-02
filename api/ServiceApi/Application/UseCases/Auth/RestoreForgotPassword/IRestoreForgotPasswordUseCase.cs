using Application.UseCases.Auth.RestoreForgotPassword.Response;
using Application.UseCases.Auth.VerifyEmail.Request;

namespace Application.UseCases.Auth.RestoreForgotPassword;

/// <summary>
/// Аутентификация при изменении пароля
/// </summary>
public interface IRestoreForgotPasswordUseCase
{  
    /// <summary>
    /// Логика обработки
    /// </summary>
    Task<RestoreForgotPasswordResponse> ExecuteAsync(VerifyEmailRequest request);
}