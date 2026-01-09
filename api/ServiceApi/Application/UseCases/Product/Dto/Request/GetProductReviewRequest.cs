namespace Application.UseCases.Product.Dto.Request;

/// <summary>
/// Входная модель получения отзыва
/// </summary>
public record GetProductReviewRequest
{
    /// <summary>
    /// Идентификатор Отзыва
    /// </summary>
    public Guid ReviewId { get; init; }
    
    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid ProductId { get; init; }
}