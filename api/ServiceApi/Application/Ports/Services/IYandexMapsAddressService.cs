using Application.UseCases.YandexMapsAddress.Dto.Response;

namespace Application.Ports.Services;

/// <summary>
/// Сервис подключения к api яндекс карт
/// </summary>
public interface IYandexMapsAddressService
{
    /// <summary>
    /// Получить список предложенных адресов
    /// </summary>
    Task<IEnumerable<GetSuggestedAddressResponse>> GetSuggestedAddressListAsync(string queryParams);
}
