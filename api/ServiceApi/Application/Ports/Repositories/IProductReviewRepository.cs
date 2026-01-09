using Domain.Product;

namespace Application.Ports.Repositories;

/// <summary>
/// Управление отзывами
/// </summary>
public interface IProductReviewRepository
{
    /// <summary>
    /// Добавить отзыв
    /// </summary>
    Task<Guid> AddProductReviewAsync(Guid userId, Guid productId, int rating, string? message);

    /// <summary>
    /// Получить отзыв
    /// </summary>
    Task<ProductReviewModel?> GetProductReviewAsync(Guid reviewId);

    /// <summary>
    /// Получить список отзывов
    /// </summary>
    Task<List<ProductReviewModel>> GetProductReviewListAsync(Guid productId);

    /// <summary>
    /// Удалить отзыв
    /// </summary>
    Task DeleteProductReviewAsync(Guid reviewId);
    
    /// <summary>
    /// Изменить видимость отзыва
    /// </summary>
    Task ChangeIsVisibleAsync(Guid reviewId, bool isVisible);
    
    /// <summary>
    /// Изменить отзыв
    /// </summary>
    Task PatchProductReviewAsync(Guid reviewId, int? rating, string? message);
}