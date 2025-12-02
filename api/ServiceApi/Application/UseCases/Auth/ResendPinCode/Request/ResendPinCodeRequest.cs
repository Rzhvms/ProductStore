namespace Application.UseCases.Auth.ResendPinCode.Request;

/// <summary>
/// Запрос на переотправку письма
/// </summary>
public record ResendPinCodeRequest
{
    /// <summary>
    /// Почта
    /// </summary>
    public string Email { get; set; }
}