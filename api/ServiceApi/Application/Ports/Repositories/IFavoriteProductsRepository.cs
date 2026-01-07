using Application.UseCases.Product.Dto.Request;
using Domain.ExtensionModels;

namespace Application.Ports.Repositories;

public interface IFavoriteProductsRepository
{
    /// <summary>
    /// Получить список избранных товаров
    /// </summary>
    Task<IEnumerable<ResultProductModel>> GetFavoriteProductListAsync(Guid userId);

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
    Task DeleteFavoriteProductListAsync(Guid userId, List<Guid> productIdList);
}