using Application.Extensions;
using Application.UseCases.Product;
using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProductController;

/// <summary>
/// Контроллер для управления избранными товарами
/// </summary>
[Authorize]
[ApiController]
[Route("api/favorite-products")]
public class FavoriteProductsController : ControllerBase
{
    private readonly IFavoriteProductsUseCase _favoriteProductsUseCase;
    
    public FavoriteProductsController(IFavoriteProductsUseCase favoriteProductsUseCase)
    {
        _favoriteProductsUseCase = favoriteProductsUseCase;
    }

    /// <summary>
    /// Получить список избранных товаров
    /// </summary>
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetProductListResponse> GetFavoriteProductListAsync()
    {
        var userId = User.GetUserId();
        return await _favoriteProductsUseCase.GetFavoriteProductListAsync(userId);
    }
    
    /// <summary>
    /// Добавить товар в избранное
    /// </summary>
    [HttpPost("{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PostFavoriteProductAsync([FromRoute] Guid productId)
    {
        var userId = User.GetUserId();
        await _favoriteProductsUseCase.PostFavoriteProductAsync(userId, productId);
        return Ok();
    }
    
    /// <summary>
    /// Удалить товар из избранного
    /// </summary>
    [HttpDelete("{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFavoriteProductAsync([FromRoute] Guid productId)
    {
        var userId = User.GetUserId();
        await _favoriteProductsUseCase.DeleteFavoriteProductAsync(userId, productId);
        return Ok();
    }
    
    /// <summary>
    /// Удалить список товаров из избранного
    /// </summary>
    [HttpPost("delete-list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFavoriteProductListAsync(DeleteFavoriteProductListRequest request)
    {
        var userId = User.GetUserId();
        await _favoriteProductsUseCase.DeleteFavoriteProductListAsync(userId, request);
        return Ok();
    }
}