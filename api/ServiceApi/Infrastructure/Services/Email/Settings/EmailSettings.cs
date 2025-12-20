namespace Infrastructure.Services.Email.Settings;

/// <summary>
/// Настройки SMTP сервера
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Хост SMTP сервера
    /// </summary>
    public string Host { get; set; } = default!;

    /// <summary>
    /// Порт SMTP сервера
    /// </summary>
    public int Port { get; set; } = 465;

    /// <summary>
    /// Использовать ли SSL
    /// </summary>
    public bool UseSsl { get; set; } = true;

    /// <summary>
    /// Имя пользователя от чьего лица отправляем
    /// </summary>
    public string Username { get; set; } = default!;

    /// <summary>
    /// Пароль SMTP сервера
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// С какого адреса отправляем письма
    /// </summary>
    public string FromAddress { get; set; } = default!;

    /// <summary>
    /// От какого имени отправляем письмо
    /// </summary>
    public string FromName { get; set; } = "Server";

    /// <summary>
    /// Имя получателя
    /// </summary>
    public string RecipientName { get; set; } = "client";
}