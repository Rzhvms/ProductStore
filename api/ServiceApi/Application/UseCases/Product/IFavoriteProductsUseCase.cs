using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;

namespace Application.UseCases.Product;

/// <summary>
/// Управление избранными товарами
/// </summary>
public interface IFavoriteProductsUseCase
{
    /// <summary>
    /// Получить список избранных товаров
    /// </summary>
    Task<GetProductListResponse> GetFavoriteProductListAsync(Guid userId, int pageNumber, int pageSize);

    /// <summary>
    /// Добавить товар в избранное
    /// </summary>
    Task PostFavoriteProductAsync(Guid userId, Guid productId);

    /// <summary>
    /// Удалить товар из избранного
    /// </summary>
    Task DeleteFavoriteProductAsync(Guid userId, Guid productId);

    /// <summary>
    /// Удалить список товаров из избранного
    /// </summary>
    Task DeleteFavoriteProductListAsync(Guid userId, DeleteFavoriteProductListRequest request);
}