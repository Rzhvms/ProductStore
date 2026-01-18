using Application.UseCases.YandexMapsAddress;
using Application.UseCases.YandexMapsAddress.Dto.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.YandexMapsAddressController;

/// <summary>
/// Контроллер для получения предложенного адреса с api Яндекс карт
/// </summary>
[ApiController]
[Route("api/suggest-address")]
public class YandexMapsAddressController : ControllerBase
{
    private readonly IYandexMapsAddressUseCase _yandexMapsAddressUseCase;

    public YandexMapsAddressController(IYandexMapsAddressUseCase yandexMapsAddressUseCase)
    {
        _yandexMapsAddressUseCase = yandexMapsAddressUseCase;
    }

    /// <summary>
    /// Получить предложенный адрес
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetSuggestedAddressListResponse> GetSuggestedAddressListAsync([FromQuery] string queryParams)
    {
        return await _yandexMapsAddressUseCase.GetSuggestedAddressListAsync(queryParams);
    }
}
