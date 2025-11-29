using Application.UseCases.Auth.ResendPinCode.Request;

namespace Application.UseCases.Auth.ResendPinCode;

/// <summary>
/// Переотправка почты
/// </summary>
public interface IResendPinCodeUseCase
{
    /// <summary>
    /// Выполнить сценарий
    /// </summary>
    Task ExecuteAsync(ResendPinCodeRequest request);
}