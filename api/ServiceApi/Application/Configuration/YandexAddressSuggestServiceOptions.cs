namespace Application.Configuration;

/// <summary>
/// Настройки конфигурации для подключения апи яндекс карт
/// </summary>
public class YandexAddressSuggestServiceOptions
{
    /// <summary>
    /// Апи ключ
    /// </summary>
    public string SuggestApiKey { get; init; } = string.Empty;
}