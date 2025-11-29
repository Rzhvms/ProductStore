namespace Application.Ports.Services.Email;

/// <summary>
/// Сервис работы с Email
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Отправить письмо подтверждения почты
    /// </summary>
    Task SendVerificationEmail(string email);

    /// <summary>
    /// Получить ключ кэша
    /// </summary>
    string GetEmailCacheKey(string email);
}