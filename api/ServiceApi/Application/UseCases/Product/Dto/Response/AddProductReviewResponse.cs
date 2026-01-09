namespace Application.UseCases.Product.Dto.Response;

/// <summary>
/// Выходная модель на добавление отзыва
/// </summary>
public record AddProductReviewResponse
{
    /// <summary>
    /// Идентификатор отзыва
    /// </summary>
    public Guid ReviewId { get; init; }
}