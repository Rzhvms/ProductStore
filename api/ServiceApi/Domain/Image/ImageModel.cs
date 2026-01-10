using Domain.Product.Enum;

namespace Domain.Image;

/// <summary>
/// Изображение продукта
/// </summary>
public record ImageModel
{
    /// <summary>
    /// Идентификатор изображения
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public Guid ProductId { get; init; }
    
    /// <summary>
    /// Путь в MinIO
    /// </summary>
    public string? ObjectPath { get; init; }
    
    /// <summary>
    /// Основная картинка
    /// </summary>
    public bool IsMain { get; set; }
    
    /// <summary>
    /// Порядок сортировки изображений
    /// </summary>
    public ImageSortOrder SortOrder { get; init; }
}