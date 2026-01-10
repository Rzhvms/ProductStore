using Application.Extensions;
using Application.UseCases.Product;
using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProductController;

/// <summary>
/// Контроллер для управлениями отзывами
/// </summary>
[ApiController]
[Route("api/review")]
public class ProductReviewController : ControllerBase
{
    private readonly IProductReviewUseCase _productReviewUseCase;
    
    public ProductReviewController(IProductReviewUseCase productReviewUseCase)
    {
        _productReviewUseCase = productReviewUseCase;
    }

    /// <summary>
    /// Добавить отзыв
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<AddProductReviewResponse> AddProductReviewAsync(AddProductReviewRequest request)
    {
        var userId = User.GetUserId();
        return await _productReviewUseCase.AddProductReviewAsync(userId, request);
    }
    
    /// <summary>
    /// Получить отзыв
    /// </summary>
    [HttpGet("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetProductReviewResponse> GetProductReviewAsync([FromRoute] Guid reviewId)
    {
        return await _productReviewUseCase.GetProductReviewAsync(reviewId);
    }
    
    /// <summary>
    /// Получить список отзывов
    /// </summary>
    [HttpGet("{productId}/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetProductReviewListResponse> GetProductReviewListAsync([FromRoute] Guid productId)
    {
        return await _productReviewUseCase.GetProductReviewListAsync(productId);
    }
    
    /// <summary>
    /// Удалить отзыв
    /// </summary>
    [HttpDelete("{reviewId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task DeleteProductReviewAsync([FromRoute] Guid reviewId)
    {
        await _productReviewUseCase.DeleteProductReviewAsync(reviewId);
    }

    /// <summary>
    /// Изменить видимость отзыва
    /// </summary>
    [Authorize]
    [HttpPatch("admin/change-visible/{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task ChangeIsVisibleAsync([FromRoute] Guid reviewId, ChangeIsVisibleRequest request)
    {
        await _productReviewUseCase.ChangeIsVisibleAsync(reviewId, request);
    }
    
    /// <summary>
    /// Изменить отзыв
    /// </summary>
    [Authorize]
    [HttpPatch("patch-review/{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task PatchProductReviewAsync([FromRoute] Guid reviewId, PatchProductReviewRequest request)
    {
        await _productReviewUseCase.PatchProductReviewAsync(reviewId, request);
    }

    /// <summary>
    /// Получить средний рейтинг товара
    /// </summary>
    [HttpGet("average-rating/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetAverageRatingResponse> GetAverageRatingAsync([FromRoute] Guid productId)
    {
        return await _productReviewUseCase.GetAverageRatingAsync(productId);
    }
}