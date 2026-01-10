using System.Text.Json;
using Application.Configuration;
using Application.Ports.Services;
using Application.UseCases.YandexMapsAddress.Dto.Response;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Yandex;

/// <inheritdoc/>
internal class YandexMapsAddressService : IYandexMapsAddressService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public YandexMapsAddressService(HttpClient httpClient, IOptions<YandexAddressSuggestServiceOptions> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.SuggestApiKey;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GetSuggestedAddressResponse>> GetSuggestedAddressListAsync(string queryParams)
    {
        var apiUrl = BuildUrl(queryParams);

        using var apiResponse = await _httpClient.GetAsync(apiUrl);
        apiResponse.EnsureSuccessStatusCode();
        
        await using var stream = await apiResponse.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);

        if (!json.RootElement.TryGetProperty("results", out var results))
        {
            return [];
        }

        var items = new List<GetSuggestedAddressResponse>();

        foreach (var x in results.EnumerateArray())
        {
            if (!x.TryGetProperty("title", out var titleProp))
            {
                continue;
            }

            if (!titleProp.TryGetProperty("text", out var titleText))
            {
                continue;
            }

            var id = x.TryGetProperty("id", out var idProp) ? idProp.GetString() : null;

            var subtitle = 
                x.TryGetProperty("subtitle", out var subProp) 
                && subProp.TryGetProperty("text", out var subText) 
                ? subText.GetString() 
                : null;

            items.Add(new GetSuggestedAddressResponse
            {
                Id = id ?? string.Empty,
                Title = titleText.GetString()!,
                Subtitle = subtitle
            });
        }

        return items;
    }
    
    /// <summary>
    /// Получение ссылки
    /// </summary>
    private string BuildUrl(string text)
    {
        return $"https://suggest-maps.yandex.ru/v1/suggest" +
               $"?apikey={Uri.EscapeDataString(_apiKey)}" +
               $"&text={Uri.EscapeDataString(text)}" +
               $"&lang=ru_RU" +
               $"&results=5";
    }
}