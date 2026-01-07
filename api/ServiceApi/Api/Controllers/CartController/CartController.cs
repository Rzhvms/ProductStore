using Application.Extensions;
using Application.UseCases.Cart;
using Application.UseCases.Cart.Dto.Request;
using Application.UseCases.Cart.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.CartController;

/// <summary>
/// Контроллер по управлению корзиной
/// </summary>
[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartUseCase _cartUseCase;

    public CartController(ICartUseCase cartUseCase)
    {
        _cartUseCase = cartUseCase;
    }

    /// <summary>
    /// Получить корзину текущего пользователя.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetCartResponse> GetCartAsync()
    {
        var userId = User.GetUserId();
        return await _cartUseCase.GetCartAsync(userId);
    }

    /// <summary>
    /// Добавить или обновить позицию в корзине. Если позиция есть — заменяем количество.
    /// </summary>
    [HttpPost("items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task AddOrUpdateItemAsync([FromBody] AddOrUpdateCartItemRequest request)
    {
        var userId = User.GetUserId();
        var item = new CartItemDto { ProductId = request.ProductId, Quantity = request.Quantity };
        await _cartUseCase.AddOrUpdateItemAsync(userId, item);
    }

    /// <summary>
    /// Удалить позицию из корзины по productId.
    /// </summary>
    [HttpDelete("items/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task RemoveItemAsync([FromRoute] Guid productId)
    {
        var userId = User.GetUserId();
        await _cartUseCase.RemoveItemAsync(userId, productId);
    }

    /// <summary>
    /// Очистить корзину.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task ClearCartAsync()
    {
        var userId = User.GetUserId();
        await _cartUseCase.ClearCartAsync(userId);
    }
    
    /// <summary>
    /// Очистка корзины после оплаты
    /// </summary>
    [HttpPost("clear-after-payment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Obsolete("Решено не использовать, чистка корзины при оформлении заказа")]
    public async Task ClearCartAfterPaymentAsync(Guid userId, Guid paymentId)
    {
        await _cartUseCase.ClearCartAfterPaymentAsync(userId, paymentId);
    }
}