namespace Application.UseCases.Product.Dto.Response;

/// <summary>
/// Выходная модель на получения списка отзывов для продукта
/// </summary>
public record GetProductReviewListResponse
{
    public List<GetProductReviewResponse>? ProductReviewList { get; init; }
}