using Application.UseCases.YandexMapsAddress.Dto.Response;

namespace Application.UseCases.YandexMapsAddress;

/// <summary>
/// UseCase для получения предложенного адреса с api Яндекс карт
/// </summary>
public interface IYandexMapsAddressUseCase
{
    /// <summary>
    /// Получение предложенных адресов
    /// </summary>
    Task<GetSuggestedAddressListResponse> GetSuggestedAddressListAsync(string queryParams);
}