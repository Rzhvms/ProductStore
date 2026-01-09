using Application.Ports.Repositories;
using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;

namespace Application.UseCases.Product;

/// <inheritdoc/>
internal class ProductReviewUseCase : IProductReviewUseCase
{
    private readonly IProductReviewRepository _productReviewRepository;

    public ProductReviewUseCase(IProductReviewRepository productReviewRepository)
    {
        _productReviewRepository = productReviewRepository;
    }
    
    /// <inheritdoc/>
    public async Task<AddProductReviewResponse> AddProductReviewAsync(Guid userId, AddProductReviewRequest request)
    {
        var id = await _productReviewRepository.AddProductReviewAsync(userId, request.ProductId, request.Rating, request.Message);
        return new AddProductReviewResponse
        {
            ReviewId = id
        };
    }
    
    /// <inheritdoc/>
    public async Task<GetProductReviewResponse> GetProductReviewAsync(Guid reviewId)
    {
        var review = await _productReviewRepository.GetProductReviewAsync(reviewId);
        return new GetProductReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            ProductId = review.ProductId,
            Rating = review.Rating,
            Message = review.Message,
            CreatedAt = review.CreatedAt,
            IsVisible = review.IsVisible
        };
    }
    
    /// <inheritdoc/>
    public async Task GetProductReviewListAsync()
    {
        
    }
    
    /// <inheritdoc/>
    public async Task DeleteProductReviewAsync()
    {
        
    }
}