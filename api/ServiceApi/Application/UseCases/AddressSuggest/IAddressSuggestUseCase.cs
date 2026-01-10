using Application.UseCases.AddressSuggest.Dto.Response;

namespace Application.UseCases.AddressSuggest;

public interface IAddressSuggestUseCase
{
    Task<IReadOnlyCollection<AddressSuggestItem>> ExecuteAsync(string query);
}
