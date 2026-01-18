using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Product.Dto.Request;

/// <summary>
/// Входная модель на добавление отзыва
/// </summary>
public record AddProductReviewRequest
{
    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid ProductId { get; init; }
    
    /// <summary>
    /// Рейтинг
    /// </summary>
    [Range(1, 5)]
    public int Rating { get; init; }
    
    /// <summary>
    /// Комментарий
    /// </summary>
    [MaxLength(300)]
    public string? Message { get; init; }
}