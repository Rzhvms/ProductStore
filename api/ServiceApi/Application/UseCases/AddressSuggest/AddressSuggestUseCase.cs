using Application.Ports.Services;
using Application.UseCases.AddressSuggest.Dto.Response;

namespace Application.UseCases.AddressSuggest;

public sealed class AddressSuggestUseCase : IAddressSuggestUseCase
{
    private readonly IAddressSuggestService _service;

    public AddressSuggestUseCase(IAddressSuggestService service)
    {
        _service = service;
    }

    public async Task<IReadOnlyCollection<AddressSuggestItem>> ExecuteAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            return Array.Empty<AddressSuggestItem>();

        return await _service.SuggestAsync(query);
    }
}
