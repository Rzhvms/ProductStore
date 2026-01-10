namespace Application.UseCases.Product.Dto.Response;

/// <summary>
/// Выходная модель получения среднего рейтинга товара
/// </summary>
public record GetAverageRatingResponse
{
    public decimal Rating { get; init; }
}
