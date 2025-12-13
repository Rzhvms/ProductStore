namespace Application.UseCases.Category.Dto.Request;

public record UpdateCategoryRequest
{
    public required string Name { get; init; }
    public Guid? ParentId { get; init; }
}