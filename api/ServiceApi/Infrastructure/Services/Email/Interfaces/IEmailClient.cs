using MimeKit;

namespace Infrastructure.Services.Email.Interfaces;

/// <summary>
/// Клиент для отправки email сообщений
/// </summary>
public interface IEmailClient
{
    /// <summary>
    /// Отправить сообщение
    /// </summary>
    Task SendAsync(MimeMessage message);
}