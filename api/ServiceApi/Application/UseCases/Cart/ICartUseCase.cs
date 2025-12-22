using Application.UseCases.Cart.Dto.Response;

namespace Application.UseCases.Cart;

/// <summary>
/// UseCase по управлению корзиной
/// </summary>
public interface ICartUseCase
{
    /// <summary>
    /// Получить корзину
    /// </summary>
    Task<GetCartResponse> GetCartAsync(Guid userId);
    
    /// <summary>
    /// Добавить или обновить элемент корзины
    /// </summary>
    Task AddOrUpdateItemAsync(Guid userId, CartItemDto item);
    
    /// <summary>
    /// Удалить элемент корзины
    /// </summary>
    Task RemoveItemAsync(Guid userId, Guid productId);
    
    /// <summary>
    /// Очистить корзину
    /// </summary>
    Task ClearCartAsync(Guid userId);
}