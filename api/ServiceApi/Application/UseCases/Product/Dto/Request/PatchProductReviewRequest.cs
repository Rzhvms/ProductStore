using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Product.Dto.Request;

/// <summary>
/// Входная модель изменения отзыва
/// </summary>
public record PatchProductReviewRequest
{
    /// <summary>
    /// Рейтинг, количество звезд
    /// </summary>
    [Range(1, 5)]
    public int? Rating { get; init; }
    
    /// <summary>
    /// Комментарий
    /// </summary>
    [MaxLength(300)]
    public string? Message { get; init; }
}