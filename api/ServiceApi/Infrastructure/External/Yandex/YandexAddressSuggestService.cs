using Application.Ports.Services;
using Application.UseCases.AddressSuggest.Dto.Response;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Infrastructure.External.Yandex.Options;

public sealed class YandexAddressSuggestService : IAddressSuggestService
{
    private readonly HttpClient _client;
    private readonly string _apiKey;

    public YandexAddressSuggestService(
        HttpClient client,
        IOptions<YandexAddressSuggestServiceOptions> options)
    {
        _client = client;
        _apiKey = options.Value.SuggestApiKey
            ?? throw new InvalidOperationException("Yandex API key is missing");
    }

    public async Task<IReadOnlyCollection<AddressSuggestItem>> SuggestAsync(string query)
{
    var url =
        $"https://suggest-maps.yandex.ru/v1/suggest" +
        $"?apikey={_apiKey}" +
        $"&text={Uri.EscapeDataString(query)}" +
        $"&lang=ru_RU" +
        $"&results=5";

    var response = await _client.GetAsync(url);
    response.EnsureSuccessStatusCode();

    await using var stream = await response.Content.ReadAsStreamAsync();
    using var json = await JsonDocument.ParseAsync(stream);

    if (!json.RootElement.TryGetProperty("results", out var results))
        return Array.Empty<AddressSuggestItem>();

    var items = new List<AddressSuggestItem>();

    foreach (var x in results.EnumerateArray())
    {
        if (!x.TryGetProperty("title", out var titleProp))
            continue;

        if (!titleProp.TryGetProperty("text", out var titleText))
            continue;

        var id =
            x.TryGetProperty("id", out var idProp)
                ? idProp.GetString()
                : null;

        var subtitle =
            x.TryGetProperty("subtitle", out var subProp) &&
            subProp.TryGetProperty("text", out var subText)
                ? subText.GetString()
                : null;

        items.Add(new AddressSuggestItem
        {
            Id = id ?? string.Empty,
            Title = titleText.GetString()!,
            Subtitle = subtitle
        });
    }

    return items;
}

}
