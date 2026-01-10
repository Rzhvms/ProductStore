namespace Application.Ports.Services;
using Application.UseCases.AddressSuggest.Dto.Response;
public interface IAddressSuggestService
{
    Task<IReadOnlyCollection<AddressSuggestItem>> SuggestAsync(string query);
}
