using Application.UseCases.Cart.Dto.Response;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для корзины
/// </summary>
public interface ICartRepository
{
    /// <summary>
    /// Получить корзину
    /// </summary>
    Task<GetCartResponse> GetCartAsync(Guid userId);
    
    /// <summary>
    /// Добавить или обновить элемент в корзине
    /// </summary>
    Task AddOrUpdateItemAsync(Guid userId, CartItemDto item);
    
    /// <summary>
    /// Удалить элемент из корзины
    /// </summary>
    Task RemoveItemAsync(Guid userId, Guid productId);
    
    /// <summary>
    /// Очистить корзину
    /// </summary>
    Task ClearCartAsync(Guid userId);
}