namespace Application.UseCases.Product.Dto.Response;

public record CreateProductResponse
{
    public Guid ProductId { get; init; }
}