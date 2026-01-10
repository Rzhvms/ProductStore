namespace Application.UseCases.AddressSuggest.Dto.Response;

public sealed class AddressSuggestItem
{
    public string Id { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string? Subtitle { get; init; }
}
