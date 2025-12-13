namespace Application.UseCases.Category.Dto.Response;

/// <summary>
/// Ответ на создание категории
/// </summary>
public record CreateCategoryResponse
{
    /// <summary>
    /// Айди категории
    /// </summary>
    public Guid Id { get; init; }
}