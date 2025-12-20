using Domain.Product;

namespace Application.UseCases.Product.Dto.Response;

public record GetProductListResponse
{
    public List<ProductModel> ProductList { get; init; }
}