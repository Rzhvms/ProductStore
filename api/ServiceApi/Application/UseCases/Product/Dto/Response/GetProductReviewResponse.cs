namespace Application.UseCases.Product.Dto.Response;

/// <summary>
/// Выходная модель получения отзыва
/// </summary>
public record GetProductReviewResponse
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }
    
    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid ProductId { get; init; }
    
    /// <summary>
    /// Рейтинг
    /// </summary>
    public int Rating { get; init; }
    
    /// <summary>
    /// Комментарий
    /// </summary>
    public string? Message { get; init; }
    
    /// <summary>
    /// Дата добавления
    /// </summary>
    public DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Скрыт ли отзыв
    /// </summary>
    public bool IsVisible { get; init; }
}