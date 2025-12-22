namespace Application.UseCases.Category.Dto.Request;

/// <summary>
/// Dto создание категории
/// </summary>
public record CreateCategoryRequest
{
    /// <summary>
    /// Название категории
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Айди родительской категории
    /// </summary>
    public Guid? ParentId { get; init; }
}