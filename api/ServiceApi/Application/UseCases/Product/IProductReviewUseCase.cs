using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;

namespace Application.UseCases.Product;

/// <summary>
/// Управление отзывами
/// </summary>
public interface IProductReviewUseCase
{
    /// <summary>
    /// Добавить отзыв
    /// </summary>
    Task<AddProductReviewResponse> AddProductReviewAsync(Guid userId, AddProductReviewRequest request);

    /// <summary>
    /// Получить отзыв
    /// </summary>
    Task<GetProductReviewResponse> GetProductReviewAsync(Guid reviewId);

    /// <summary>
    /// Получить список отзывов
    /// </summary>
    Task GetProductReviewListAsync();

    /// <summary>
    /// Удалить отзыв
    /// </summary>
    Task DeleteProductReviewAsync();
}