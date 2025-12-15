namespace Application.UseCases.Category.Dto.Response;

public record GetCategoryResponse
{
    public Guid? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public Guid? ParentCategoryId { get; init; }
}