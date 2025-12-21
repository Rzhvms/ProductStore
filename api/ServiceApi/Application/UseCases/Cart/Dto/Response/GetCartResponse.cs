namespace Application.UseCases.Cart.Dto.Response;

public record GetCartResponse
{
    public List<CartItemDto> Items { get; init; } = new();
}