namespace Application.UseCases.Product.Dto.Request;

/// <summary>
/// Входная модель изменения поля видимости отзыва
/// </summary>
public record ChangeIsVisibleRequest
{
    public bool IsVisible { get; init; }
}