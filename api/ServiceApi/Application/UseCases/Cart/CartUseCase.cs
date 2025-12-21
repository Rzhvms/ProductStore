using Application.Ports.Repositories;
using Application.UseCases.Cart.Dto.Response;

namespace Application.UseCases.Cart;

/// <inheritdoc/>
public class CartUseCase : ICartUseCase
{
    private readonly ICartRepository _cartRepository;

    public CartUseCase(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    /// <inheritdoc/>
    public async Task<GetCartResponse> GetCartAsync(Guid userId)
    {
        return await _cartRepository.GetCartAsync(userId);
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateItemAsync(Guid userId, CartItemDto item)
    {
        await _cartRepository.AddOrUpdateItemAsync(userId, item);
    }

    /// <inheritdoc/>
    public async Task RemoveItemAsync(Guid userId, Guid productId)
    {
        await _cartRepository.RemoveItemAsync(userId, productId);   
    }

    /// <inheritdoc/>
    public async Task ClearCartAsync(Guid userId)
    {
        await _cartRepository.ClearCartAsync(userId);   
    }
}