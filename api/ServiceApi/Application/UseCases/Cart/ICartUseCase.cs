using Application.UseCases.Cart.Dto.Response;

namespace Application.UseCases.Cart;

/// <summary>
/// UseCase по управлению корзиной
/// </summary>
public interface ICartUseCase
{
    Task<GetCartResponse> GetCartAsync(Guid userId);
    Task AddOrUpdateItemAsync(Guid userId, CartItemDto item);
    Task RemoveItemAsync(Guid userId, Guid productId);
    Task ClearCartAsync(Guid userId);
}