using Application.UseCases.Auth.ResendPinCode.Request;

namespace Application.UseCases.Auth.ResendPinCode;

/// <summary>
/// Переотправить письмо верификации
/// </summary>
public interface IResendPinCodeUseCase
{
    /// <summary>
    /// Переотправить письмо верификации
    /// </summary>
    Task ExecuteAsync(ResendPinCodeRequest request);
}