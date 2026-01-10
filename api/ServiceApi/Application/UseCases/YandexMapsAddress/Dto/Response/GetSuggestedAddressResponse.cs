namespace Application.UseCases.YandexMapsAddress.Dto.Response;

/// <summary>
/// Выходная модель получения предложенного адреса с api Яндекс карт
/// </summary>
public record GetSuggestedAddressResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public string? Id { get; init; }
    
    /// <summary>
    /// Заголовок
    /// </summary>
    public string? Title { get; init; }
    
    /// <summary>
    /// Подзаголовок
    /// </summary>
    public string? Subtitle { get; init; }
}
