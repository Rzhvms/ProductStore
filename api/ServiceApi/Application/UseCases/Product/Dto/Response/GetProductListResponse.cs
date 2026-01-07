using Domain.Product;

namespace Application.UseCases.Product.Dto.Response;

/// <summary>
/// Выходная модель для получения списка продуктов на клиентской части
/// </summary>
public record GetProductListResponse
{
    /// <summary>
    /// Список продуктов
    /// </summary>
    public List<GetProductResponse> ProductList { get; init; }
}