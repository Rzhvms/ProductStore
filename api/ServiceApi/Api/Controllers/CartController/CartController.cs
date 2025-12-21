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
    public async Task<ActionResult<GetCartResponse>> GetCartAsync()
    {
        var userId = User.GetUserId();
        var cart = await _cartUseCase.GetCartAsync(userId);
        return Ok(cart);
    }

    /// <summary>
    /// Добавить или обновить позицию в корзине. Если позиция есть — заменяем количество.
    /// </summary>
    [HttpPost("items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOrUpdateItemAsync([FromBody] AddOrUpdateCartItemRequest request)
    {
        var userId = User.GetUserId();
        
        if (request.ProductId == Guid.Empty) 
            return BadRequest("ProductId is required.");
        
        if (request.Quantity <= 0) 
            return BadRequest("Quantity must be >= 1.");

        var item = new CartItemDto { ProductId = request.ProductId, Quantity = request.Quantity };
        await _cartUseCase.AddOrUpdateItemAsync(userId, item);

        return Ok();
    }

    /// <summary>
    /// Удалить позицию из корзины по productId.
    /// </summary>
    [HttpDelete("items/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveItemAsync([FromRoute] Guid productId)
    {
        var userId = User.GetUserId();

        await _cartUseCase.RemoveItemAsync(userId, productId);
        return NoContent();
    }

    /// <summary>
    /// Очистить корзину.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ClearCartAsync()
    {
        var userId = User.GetUserId();

        await _cartUseCase.ClearCartAsync(userId);
        return NoContent();
    }
}