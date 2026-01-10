using Application.Ports.Services;
using Application.UseCases.YandexMapsAddress.Dto.Response;

namespace Application.UseCases.YandexMapsAddress;

/// <inheritdoc/>
internal class YandexMapsAddressUseCase : IYandexMapsAddressUseCase
{
    private readonly IYandexMapsAddressService _service;

    public YandexMapsAddressUseCase(IYandexMapsAddressService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    public async Task<GetSuggestedAddressListResponse> GetSuggestedAddressListAsync(string queryParams)
    {
        if (string.IsNullOrWhiteSpace(queryParams) || queryParams.Length < 3)
        {
            return new GetSuggestedAddressListResponse();
        }

        var addressList = await _service.GetSuggestedAddressListAsync(queryParams);
        
        return new GetSuggestedAddressListResponse
        {
            SuggestedAddressList = addressList
        };
    }
}
