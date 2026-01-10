namespace Application.UseCases.YandexMapsAddress.Dto.Response;

/// <summary>
/// Выходная модель получения списка предложенных адресов с api Яндекс карт
/// </summary>
public record GetSuggestedAddressListResponse
{
    /// <summary>
    /// Список предложенных адресов
    /// </summary>
    public IEnumerable<GetSuggestedAddressResponse> SuggestedAddressList { get; init; }
}