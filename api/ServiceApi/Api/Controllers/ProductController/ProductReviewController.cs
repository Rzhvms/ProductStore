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
    public async Task<AddProductReviewResponse> AddProductReviewAsync(AddProductReviewRequest request)
    {
        var userId = User.GetUserId();
        return await _productReviewUseCase.AddProductReviewAsync(userId, request);
    }
    
    /// <summary>
    /// Получить отзыв
    /// </summary>
    [HttpGet("{reviewId}")]
    public async Task<GetProductReviewResponse> GetProductReviewAsync([FromRoute] Guid reviewId)
    {
        return await _productReviewUseCase.GetProductReviewAsync(reviewId);
    }
    
    /// <summary>
    /// Получить список отзывов
    /// </summary>
    [HttpGet("list")]
    public async Task GetProductReviewListAsync()
    {
        
    }
    
    /// <summary>
    /// Удалить отзыв
    /// </summary>
    [HttpDelete("{id}")]
    public async Task DeleteProductReviewAsync()
    {
        
    }
}