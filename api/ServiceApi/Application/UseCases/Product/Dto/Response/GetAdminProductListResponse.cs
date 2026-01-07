namespace Application.UseCases.Product.Dto.Response;

/// <summary>
/// Выходная модель получения списка продуктов на админ панели
/// </summary>
public record GetAdminProductListResponse
{
    /// <summary>
    /// Список продуктов
    /// </summary>
    public List<GetAdminProductResponse> ProductList { get; init; }
}